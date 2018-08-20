
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
