
var finalZip = new JSZip();
var rateLimit = null;
var rateLimited = false;
var totalSteps = 0;
var finishedSteps = 0;
var setupList;
var donotcontinue = false;
var retry = 0;

var thresholds = {
	0: true,
	1: false,
	2: false,
	3: false,
	4: false,
	5: false,
	6: false,
	7: false,
	8: false,
	9: false,
	10: false,
	11: false,
	12: false,
	13: false,
	14: false,
	15: false,
	16: false,
	17: false,
	18: false,
	19: false,
	20: false
}

function downloadZip() {
	return finalZip.generateAsync({ type: "blob", compression: "STORE" }, function (meta) {
		if (thresholds[Math.floor(meta.percent / 5)] == false) {
			thresholds[Math.floor(meta.percent / 5)] = true;
			DotNet.invokeMethodAsync("SDSetupBlazor", "ChangeProgress", meta.percent.toFixed(0));
		}
	})
		.then(function (blob) {
			if (window.navigator.userAgent.indexOf("Edge") > -1) {
				window.navigator.msSaveBlob(blob, "SD.SETUP.SWITCH.zip");
			} else {
				saveAs(blob, "SD.SETUP.SWITCH.zip");
			}
			return 1;
	});
}

function startSetup(data){
	updateRateLimit();
	if(data.start){
		start = data.start;
	}
	if(zipname == undefined && data.zipname){
		zipname = data.zipname;
	}
	download_msg = toastr["warning"]("Once all downloads finish, click 'Download Zip' and extract everything inside the given zip into your SD Card");

	try{
		$('#guide_btn').attr("href",start);
		setupList["otherapp"].url = updatePayload();
		setupList["Soundhax"].url = soundhaxURL();
	}finally{

		$("#inner1").hide();
		$("#inner2").show();

		readList(data.steps);
	}
}

function readList(list){
	for(var i=0;i<list.length;i++){
		var itemName = list[i];
		if(itemName && setupList.hasOwnProperty(itemName)) {
			evaluateItem(itemName);
		}
	}
}

function evaluateItem(itemName) {
	var item = setupList[itemName];
	if(checkReq(item.require)){
		switch(item.type) {
			case "github":
				runGithub(item,itemName);
				break;
			case "direct":
				runDirect(item,itemName);
				break;
			case "torrent":
				if(item.urls){

				}else{
					torrent(item);
				}
				break;
			case "list":
				if(zipname == undefined && item.zipname == undefined){
					zipname = item.zipname;
				}
				readList(item.steps);
				break;
		}
	}
}

function evaluateStep(step, data, name) {
	switch(step.type){
		case "extractFile":
			if(step.fileExtract) {
				if(step.newName == undefined){
					step.newName = step.fileExtract;
				}
				getFileBuffer_zip(data, step.fileExtract, step.path, step.newName);
			} else if(step.files) {
				step.files.forEach(function(fileStep) {
					if(fileStep.newName == undefined){
						fileStep.newName = fileStep.file;
					}
					getFileBuffer_zip(data, fileStep.file, fileStep.path, fileStep.newName);
				});
			} else {
				if(step.fileDelete){
					if(step.fileDelete.files){
						step.fileDelete.files.forEach(function (fileStep){
							deletefile_zip(data, fileStep);
						})
					}else{
						deletefile_zip(data, step.fileDelete);
					}
				}
				extractZip(data, step.path, step.removePath);
			}
			break;

		case "extractFolder":
			extractFolder(data, step.folder, step.path);
			break;

		case "addFile":
			if(step.name){
				addFile(data, step.path, step.name);
			}else{
				addFile(data, step.path, step.file);
			}

			break;

		case "folder":
			folder(step.name);
			break;
			// add more
	}

	finishedSteps++; // kinda
	if(totalSteps === finishedSteps) {
		$('#download_btn').text("Download");
		$('#download_btn').click(function() {
			downloadZip();
		});
	}

	//element.childNodes[element.childNodes.length -1].innerText = "(Added!)";
}

// Prepares files and runs each step passing the downloaded files.
function runGithub(item,name) {
	getGithubRelease(item, function(err, info) {
		item.steps.forEach(function(step) {
			totalSteps++;
			if(step.type==="folder"){
				evaluateStep(step, null, name);
				return;
			};
			var asset = getGithubAsset(info.assets, step.file);
			if(asset === null) {
				console.log("no asset found for " + step.file);
				return;
			}

			getFileBuffer_url(corsURL(asset.browser_download_url), name, function(data) {
				evaluateStep(step, data, name);
			});
		});
	});
}

function runDirect(item,name) {
	totalSteps++;

	getFileBuffer_url(corsURL(item.url), name, function(data) {
		item.steps.forEach(function(step) {
			evaluateStep(step, data, name);
		});
	});
}

function getGithubRelease(options, callback) {
	callback = callback || function(){};

	if(rateLimited) {
		callback(new Error("Rate limited lol :p"));
		return;
	}

	var defaults = {
		repo: "",
		version: "latest"
	};

	options = $.extend(defaults, options);
	if(!options.repo) {
		callback(new Error("Repo name is required"), null);
		return;
	}

	var url = "https://api.github.com/repos/" + options.repo + "/releases";
	if(options.version === "latest") {
		url += "/latest";
	} else if(options.version !== "") {
		url += "/tags/" + options.version;
	}

	$.getJSON(url, function(data) {
		if(options.version === ""){
			var versionCount = 0;
			while(data[versionCount].assets.length == 0){
				versionCount += 1;
			}
			data = data[versionCount];
		}

		callback(null, data);
	}).fail(function(jqXHR) {
		//rateLimit(jqXHR);
	});
}

function getFileBuffer_url(url, name) {
	console.log("Downloading " + url);
	return fetch(url).then((Response) => {
		if (Response.ok) {
			return Response.blob();
		} else {
			console.log("e1 js");
			console.log("File download failed!");
			return;
		}
	}).then((blob) => {
		console.log("Downloaded " + url);
		return blob;
	});
}

function getFileBuffer_zip(data, originalName, path, newName){
	//should work with relative and not absolute names (not implemented)
	newName = newName || originalName;

	try {
		data.file(originalName).async("arraybuffer").then(function(content){
			addFile(content, path, newName);
		});
	} catch(e) {
		console.log("Could not get " + originalName + " from some zip file");
		console.log(data);
	}

}

function extractFolder(data, folder, path){
	var file_count2 = 0;

	Object.keys(data.files).forEach(function(filename){
		var file = data.files[filename];
		if (file.dir || !filename.startsWith(folder)) {
			file_count2++;
			return;
		}

		file.async("arraybuffer").then(function(content) {
			file_count2++;
			addFile(content, path, filename);
		});
	});
}

function extractZip(data, path, removePath){
	//progress(bufferName, bufferName + ": Extracting");
	var fileCount = 0;

	Object.keys(data.files).forEach(function(key){
		var file = data.files[key];
		var filename = file.name;
		if(removePath != ""){
			filename = filename.replace(removePath + "/", "");
		};

		if (file.dir) {
			fileCount++;
			return;
		}

		file.async("arraybuffer").then(function(content) {
			fileCount++;
			addFile(content, path, filename);
		});
	});
}

function addFile(buffer, path, filename) {
	if (path === "") {
		finalZip.file(filename, buffer);
	} else {
		finalZip.folder(path).file(filename, buffer);
	}
}

function deletefile_zip(data, filename){
	data.remove(filename);
}

function folder(name){
	finalZip.file(name + "/dummy.txt", "i love ice cream");
	finalZip.remove(name + "/dummy.txt");
}

function loadRateLimit() {
	$.getJSON("https://api.github.com/rate_limit", function(data){
		rateLimit = data.resources.core;
		updateRateLimit(true);
		setTimeout(loadRateLimit, 20000);
	});
}

function updateRateLimit(noTimeout) {
	if(typeof noTimeout === "undefined") {
		noTimeout = false;
	}

	if(!rateLimit) {
		loadRateLimit();
		return;
	}

	var reset = (new Date(rateLimit.reset * 1000));
	var now = (new Date()) * 1;
	var delta = Math.floor((reset - now) / 1000);
	/*$('#rl').text("Rate limit: " + delta + " seconds until reset. ")
            .append(rateLimit.remaining + "/" + rateLimit.limit + " remaining");*/

	rateLimited = rateLimit.remaining === 0;
	if(delta <= 0) {
		loadRateLimit();
	}

	if(!noTimeout) {
		setTimeout(updateRateLimit, 500);
	}
}

function corsURL(url) {
	return "https://cors-anywhere.herokuapp.com/" + url;
}

function getGithubAsset(assets, filename) {
	if(assets === null) {
		return null;
	}

	var keys = Object.keys(assets);
	for(var key in keys) {
		if(!keys.hasOwnProperty(key)) continue;

		var asset = assets[key];
		if(asset.name.indexOf(filename) > -1 || asset.name === filename){
			return asset;
		}
	}

	return null;
}

function generateUUID() {
	var d = new Date().getTime();
	if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
		d += performance.now();
	}
	return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
		var r = (d + Math.random() * 16) % 16 | 0;
		d = Math.floor(d / 16);
		return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
	});
}

window.js_interop = {
	interop_downloadFile: function () {
		downloadZip();
		return 1;
	},
	interop_BrowserCompatCheck: function () {
		if (window.navigator.userAgent.indexOf("Edge") > -1) {
			DotNet.invokeMethodAsync("SDSetupBlazor", "BrowserNotCompatible", 1);
			return 0;
		}
		if (window.navigator.userAgent.indexOf('Trident/') > -1) {
			DotNet.invokeMethodAsync("SDSetupBlazor", "BrowserNotCompatible", 2);
			return 0;
		}
		if (window.navigator.userAgent.indexOf('MSIE ') > -1) {
			DotNet.invokeMethodAsync("SDSetupBlazor", "BrowserNotCompatible", 2);
			return 0;
		}
		return 0;
	}
};

window.interop_addFile = (data) => {
	var uuid = generateUUID();
	return new Promise((resolve, reject) => {
		return getFileBuffer_url(data[0], uuid).then(function (blob) {
			if (blob != null) {
				addFile(blob, data[1], data[2]);
				retry = 0;
				resolve(1);
			}
		}).catch(function () {
			if (retry < 3) {
				console.log("File download failed, retrying...");
				retry++;
				resolve(window.interop_addFile(data));
			} else {
				console.log("File download failed too many times, cancelling.");
				resolve(2);
			}
		});
	});
};

window.interop_downloadZip = () => {
	return downloadZip();
}

window.mobileAndTabletcheck = function () {
	var check = false;
	(function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
	return check;
};
