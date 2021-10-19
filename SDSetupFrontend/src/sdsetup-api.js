import './sdsetup-typedef';

const BACKEND_URL = "http://files.sdsetup.com/api/v2/"

const ENDPOINT_LATEST_PACKAGESET = BACKEND_URL + "files/latestpackageset/";
const ENDPOINT_MANIFEST = BACKEND_URL + "files/manifest/{id}";
const ENDPOINT_REQUEST_BUNDLE = BACKEND_URL + "files/requestbundle";
const ENDPOINT_BUNDLE_PROGRESS = BACKEND_URL + "files/bundleprogress/{id}";
const ENDPOINT_DOWNLOAD_BUNDLE = BACKEND_URL + "files/downloadbundle/{id}";
const ENDPOINT_DIRECT_DOWNLOAD_BUNDLE = BACKEND_URL + "files/ddl/{packageset}/{name}";

const packageVisibilityOverrides = {};

let defaultErrorHandler;
let defaultBundleProgressHandler;
let defaultBundleSuccessHandler;

let localId = uuid4();
let manifest = {};
let latestPackageset = "";
let rerender;
let isBundlingInProgress = false;
/** @type {BundlerProgress} */
let bundlerProgress = {}
let bundlerUuid = "";

function uuid4() {
    return 'xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : ((r & 0x3) | 0x8);
        return v.toString(16);
    });
}

function handleError(err) {
    console.error(err);
    if (typeof(defaultErrorHandler) === 'function') defaultErrorHandler(err);
}

function handleProgress(progress) {
    if (typeof(defaultBundleProgressHandler) === 'function') defaultBundleProgressHandler(progress);
}

/**
 * @param {BundleResult} result 
 */
function handleSuccess(result) {
    if (typeof(defaultBundleSuccessHandler) === 'function') defaultBundleSuccessHandler(result);
}

async function fetchJson(endpoint) {
    return fetch(endpoint).then(res => {
        if (res.ok) return res.json();
        else {
            return res.text().then(msg =>  {
                let rawMessage = msg;
                if (msg.includes("<html>")) msg = res.statusText;
                return new ApiError(res.status, msg, "fetchJson", rawMessage, res.url);
            });
        }
    }).catch(err => {
        return new ApiError(-2, err.message, "fetchJson", err.stack, endpoint);
    });;
}

async function fetchString(endpoint) {
    return fetch(endpoint).then(res => {
        if (res.ok) return res.text();
        else {
            return res.text().then(msg =>  {
                let rawMessage = msg;
                if (msg.includes("<html>")) msg = res.statusText;
                return new ApiError(res.status, msg, "fetchString", rawMessage, res.url);
            });
        }
    }).catch(err => {
        return new ApiError(-2, err.message, "fetchString", err.stack, endpoint);
    });
}

async function postAndFetchString(endpoint, body) {
    const opts = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    };

    return fetch(endpoint, opts).then(res => {
        if (res.ok) return res.text();
        else {
            return res.text().then(msg =>  {
                let rawMessage = msg;
                if (msg.includes("<html>")) msg = res.statusText;
                return new ApiError(res.status, msg, "postAndFetchString", rawMessage, res.url);
            });
        }
    }).catch(err => {
        return new ApiError(-2, err.message, "postAndFetchString", err.stack, endpoint);
    });;
}

async function postAndFetchJson(endpoint, body) {
    const opts = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    };

    return fetch(endpoint, opts).then(res => {
        if (res.ok) return res.json();
        else {
            return res.text().then(msg =>  {
                let rawMessage = msg;
                if (msg.includes("<html>")) msg = res.statusText;
                return new ApiError(res.status, msg, "postAndFetchJson", rawMessage, res.url);
            });
        }
    }).catch(err => {
        return new ApiError(-2, err.message, "postAndFetchJson", err.stack, endpoint);
    });;
}

export function setDefaultErrorHandler(func) {
    defaultErrorHandler = func;
}

export function setDefaultBundleProgressHandler(func) {
    defaultBundleProgressHandler = func;
}

export function setDefaultBundleSuccessHandler(func) {
    defaultBundleSuccessHandler = func;
}

export function setForceUpdate(func) {
    rerender = func;
}

export async function fetchLatestManifest() {
    let ps;
    let man;

    ps = await fetchString(ENDPOINT_LATEST_PACKAGESET);
    if (ps.error) {
        handleError(ps.withLocation("fetchLatestManifest"));
    } else {
        latestPackageset = ps;
        man = await fetchJson(ENDPOINT_MANIFEST.replace("{id}", latestPackageset));

        if (man.error) {
            handleError(manifest.withLocation("fetchLatestManifest"));
        } else {
            manifest = man;
        }
    }
}

/**
 * 
 * @returns {Manifest}
 */
export function getManifest() {
    return manifest;
}

/**
 * 
 * @returns {string}
 */
export function getCopyrightText() {
    const manifest = getManifest();
    return manifest.copyright.replace("$", "<br />");
}

/**
 * 
 * @param {Bundle} bundle 
 * @returns {string}
 */
export function getDirectForBundle(bundle) {
    return ENDPOINT_DIRECT_DOWNLOAD_BUNDLE.replace("{packageset}", latestPackageset).replace("{name}", bundle.name);
}

/**
 * @param {string} id 
 * @returns {Package}
 */
export function getPackageById(id) {
    const manifest = getManifest();
    return manifest.packages[id]; 
}

/**
 * 
 * @param {string} id 
 * @returns {Platform} 
 */
export function getPlatformById(id) {
    const manifest = getManifest();
    return manifest.platforms[id];
}

/**
 * 
 * @returns {string[]}
 */
export function getAllPlatforms() {
    const manifest = getManifest();
    return Object.keys(manifest.platforms);
}

/**
 * 
 * @returns {string[]}
 */
export function getVisiblePlatforms() {
    const manifest = getManifest();
    return Object.keys(manifest.platforms).filter((id) => {
        const platform = manifest.platforms[id];
        return platform.visible;
    });
}

/**
 * @param {Section} section
 * @returns {Package[]} 
 */
export function getVisiblePackagesForSection(section) {
    const packages = [];
    for (const cat of Object.values(section.categories)) {
        for (const sub of Object.values(cat.subcategories)) {
            for (const pkgid of sub.packages) {
                const pkg = getPackageById(pkgid);
                if (canShow(pkg)) {
                    packages.push(pkg);
                }
            }
        }
    }

    return packages;
}

/**
 * 
 * @param {string} platform 
 * @returns {Bundle[]}
 */
export function getBundlesForPlatform(platform) {
    const manifest = getManifest();
    return manifest.platforms[platform].bundles;
}

/**
 * 
 * @param {object} obj 
 * @returns {boolean}
 */
export function canShow(obj) {
    let id = obj.id;

    if (packageVisibilityOverrides[id]) return true;
    
    if (!obj.visible) return false;
    else if (obj.when.length === 0) return true;
    else {
        if (obj.whenMode === 0) { //all
            for (const packageId of obj.when) {
                const pkg = getPackageById(packageId);
                if (!pkg || !pkg.checked) return false;
            }

            return true;
        } else if (obj.whenMode === 1) { //any
            for (const packageId of obj.when) {
                const pkg = getPackageById(packageId);
                if (pkg && pkg.checked) return true;
            }

            return false;
        }
    }

    return false;
}

/**
 * 
 * @param {string} packageId 
 * @param {boolean} checked 
 * @param {boolean} dirty 
 */
export function selectPackage(packageId, checked, dirty) {
    const pkg = getPackageById(packageId);
    pkg.checked = checked;
    if (!dirty) rerender();
}

/**
 * 
 * @param {string[]} packages 
 */
export function selectPackages(packages) {
    const manifest = getManifest();
    for (const pkg of Object.values(manifest.packages)) {
        pkg.checked = packages.includes(pkg.id);
    }
    rerender();
}

/**
 * 
 * @param {string} platform 
 * @returns {number}
 */
export function countSelectedPackages(platform) {
    const manifest = getManifest()
    if (!manifest) return 0;
    else return getValidatedPackageList(platform).length;
}

/**
 * 
 * @param {string} id 
 */
export function forceShow(id) {
    packageVisibilityOverrides[id] = true;
    rerender();
}

/**
 * 
 * @returns {Package[]}
 */
export function getCreditsPackages() {
    const manifest = getManifest();
    return Object.values(manifest.packages).filter((pkg) => {
        return pkg.showsInCredits;
    });
}

/**
 * 
 * @param {string} platform 
 * @returns {string[]}
 */
export function getPackageList(platform) {
    const packages = Object.values(manifest.packages).filter((pkg) => {
        return pkg.checked && pkg.platform === platform;
    });

    return packages;
}

/**
 * 
 * @param {string} platform 
 * @returns {string[]}
 */
export function getValidatedPackageList(platform) {
    const manifest = getManifest(); 
    let packages = getPackageList(platform);
    packages = packages.filter((pkg) => {
        const section = manifest.platforms[pkg.platform].packageSections[pkg.section];
        const category = section.categories[pkg.category];
        const subcategory = category.subcategories[pkg.subcategory];

        const result = 
            canShow(pkg) &&
            canShow(section) &&
            canShow(category) &&
            canShow(subcategory);

        return result;
    });

    return packages.map((pkg) => {
        return pkg.id;
    });
}

/**
 * 
 * @param {string} platform 
 */
export async function requestBundle(platform) {
    let packages;

    bundlerProgress = {
        progress: 0,
        total: 1,
        currentTask: "Asking the server to create your bundle.",
        isComplete: false,
        success: false
    }
    isBundlingInProgress = true;
    handleProgress(bundlerProgress);

    packages = getValidatedPackageList(platform);
    
    bundlerUuid = await postAndFetchString(ENDPOINT_REQUEST_BUNDLE, {clientId: localId, packageset: latestPackageset, packages: packages})

    checkProgressUntilComplete(platform, packages);
}

async function checkProgressUntilComplete(platform, packages) {
    const progressEndpoint = ENDPOINT_BUNDLE_PROGRESS.replace("{id}", bundlerUuid);
    bundlerProgress = await fetchJson(progressEndpoint);

    if (bundlerProgress.error) {
        handleError(bundlerProgress.withLocation("checkProgressUntilComplete"));
    } else if (bundlerProgress.isComplete && !bundlerProgress.success) {
        handleError(new ApiError(
            -3,
            bundlerProgress.currentTask,
            "checkProgressUntilComplete",
            bundlerProgress.currentTask,
            progressEndpoint
        ));
    }else if (bundlerProgress.isComplete) {
        downloadCompletedBundle(platform, packages);
    } else {
        handleProgress(bundlerProgress);
        setTimeout(() => { checkProgressUntilComplete(platform, packages); }, 200);
    }
}

async function downloadCompletedBundle(platform, packages) {
    const url = ENDPOINT_DOWNLOAD_BUNDLE.replace("{id}", bundlerUuid);
    isBundlingInProgress = false;
    handleSuccess(new BundleResult(url, platform, packages));
}

/**
 * 
 * @returns {boolean}
 */
export function isBundling() {
    return isBundlingInProgress;
}

/**
 * 
 * @returns {BundlerProgress}
 */
export function getBundlerProgress() {
    return bundlerProgress;
}

/**
 * 
 * @returns {DonationInfo}
 */
 export function getDonationInfo() {
    const manifest = getManifest();
    return manifest.donationInfo;
}

export class ApiError {
    constructor(code, message, location, rawMessage, url) {
        this.error = true;
        this.code = code;
        this.location = location;
        this.message = message;
        this.rawMessage = rawMessage;
        this.url = url;
        this.clientid = localId;
        this.bundlerid = bundlerUuid;
        this.ua = navigator.userAgent;
    }

    withLocation(loc) {
        this.location = loc + "/" + this.location; 
        return this;
    }
}

export class BundleResult {
    constructor(bundleUrl, platform, packages) {
        this.bundleUrl = bundleUrl;
        this.platform = platform;
        this.packages = packages;
    }
}