var finalBlob = null;
var dl = null
var rateLimit = null;
var rateLimited = false;
var totalSteps = 0;
var finishedSteps = 0;
var setupList;
var donotcontinue = false;
var retry = 0;
var outputName = "SDSetup.zip";




function downloadZip() {
	return new Promise((resolve, reject) => {
		zip.createWriter(new zip.BlobWriter("application/zip"), function (writer) {
			var f = 0;

			function nextFile(f) {
				fblob = Object.entries(blobs)[f];
				console.log("Zipping " + fblob[0]);
				writer.add(fblob[0], new zip.BlobReader(fblob[1]), function () {
					// callback
					f++;
					DotNet.invokeMethodAsync("SDSetupBlazor", "AddProgress");
					if (f < Object.entries(blobs).length) {
						nextFile(f);
					} else close();
				});
			}

			function close() {
				// close the writer
				writer.close(function (blob) {

                    if (getBrowserCompatInfo() === 3) {
                        console.log("Firefox browser detected");
                        if (navigator.browserSpecs.version > 61) {
                            console.log("Firefox version " + navigator.browserSpecs.version + " supports zipjs blob download");
                            saveAs(blob, outputName);
                            blobs = [];
                            resolve("");
                        } else {
                            console.log("Firefox version " + navigator.browserSpecs.version + " does not support zipjs blob download");
                            resolve(window.URL.createObjectURL(blob));
                        }

                    } else {
                        console.log("Non-Firefox detected");
                        saveAs(blob, outputName);
                        blobs = [];
                        resolve("");
                    }
				});
			}

			nextFile(f);

		}, function (error) { console.log(error); }, true);
	});
}

function save(blob, filename) {
	var a = document.createElement("a");
	a.style = "display: none";
		url = window.URL.createObjectURL(blob);
	a.href = url;
	a.download = filename;
	document.body.appendChild(a);
	a.click();  
}

function getFileBuffer_url(url, name) {
	console.log("Downloading " + url);
	return fetch(url).then((Response) => {
		if (Response.ok) {
			console.log("Response OK!");
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

window.interop_generateZip = (url) => {
	var uuid = generateUUID();
	return new Promise((resolve, reject) => {
		return getFileBuffer_url(url, uuid).then(function (blob) {
			if (blob !== null) {
				finalBlob = blob;
				finalURL = URL.createObjectURL(finalBlob);
				retry = 0;
				resolve(finalURL);
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

window.mobileAndTabletcheck = function () {
	var check = false;
	(function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
	return check;
};

window.scrollToTop = function () {
	window.scrollTo(0, 0);
	return 0;
};

window.getScrollPos = function () {
	return window.scrollY;
};

window.setScrollPos = function (pos) {
	console.log(pos);
	console.log(window.innerHeight);
	window.scrollTo(0, pos);
	return 0;
};

const readUploadedFileAsText = (inputFile) => {
	const temporaryFileReader = new FileReader();
	return new Promise((resolve, reject) => {
		temporaryFileReader.onerror = () => {
			temporaryFileReader.abort();
		};
		temporaryFileReader.addEventListener("load", function () {
			var data = {
				content: temporaryFileReader.result.split(',')[1]
			};
			resolve(data);
		}, false);
		try {
			temporaryFileReader.readAsDataURL(inputFile.files[0]);
		} catch (err) {
			temporaryFileReader.abort();
			var data = {
				content: "Problem parsing input file."
			};
			resolve(data);
		}
		
	});
};

window.getFileData = function (inputFile) {
	var expr = "#" + inputFile.replace(/"/g, '');
	return readUploadedFileAsText($(expr)[0]);
}