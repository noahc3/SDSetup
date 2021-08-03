const BACKEND_URL = "http://files.sdsetup.com/api/v2/"
const ENDPOINT_LATEST_PACKAGESET = BACKEND_URL + "files/latestpackageset/";
const ENDPOINT_MANIFEST = BACKEND_URL + "files/manifest/{id}"
const ENDPOINT_REQUEST_BUNDLE = BACKEND_URL + "files/requestbundle"
const ENDPOINT_BUNDLE_PROGRESS = BACKEND_URL + "files/bundleprogress/{id}"
const ENDPOINT_DOWNLOAD_BUNDLE = BACKEND_URL + "files/downloadbundle/{id}"

const packageVisibilityOverrides = {};
let manifest = {};
let latestPackageset = "";
let rerender;
let modalRerender;
let isBundlingInProgress = false;
let bundlerProgress = {}
let bundlerUuid = "";

async function fetchJson(endpoint) {
    return fetch(endpoint).then(res => res.json());
}

async function fetchString(endpoint) {
    return fetch(endpoint).then(res => res.text());
}

async function postAndFetchString(endpoint, body) {
    const opts = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    };

    return fetch(endpoint, opts).then(res => res.text());
}

async function postAndFetchJson(endpoint, body) {
    const opts = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    };

    return fetch(endpoint, opts).then(res => res.json());
}

export function setForceUpdate(func) {
    rerender = func;
}

export function setModalUpdate(func) {
    modalRerender = func;
}

export async function fetchLatestManifest() {
    latestPackageset = await fetchString(ENDPOINT_LATEST_PACKAGESET);
    manifest = await fetchJson(ENDPOINT_MANIFEST.replace("{id}", latestPackageset));
    console.log(manifest);
}

export function getManifest() {
    return manifest;
}

export function getCopyrightText() {
    const manifest = getManifest();
    return manifest.copyright.replace("$", "<br />");
}

export function getPackageById(id) {
    const manifest = getManifest();
    return manifest.packages[id]; 
}

export function getPlatformById(id) {
    const manifest = getManifest();
    return manifest.platforms[id];
}

export function getAllPlatforms() {
    const manifest = getManifest();
    return Object.keys(manifest.platforms);
}

export function getVisiblePlatforms() {
    const manifest = getManifest();
    return Object.keys(manifest.platforms).filter((id) => {
        const platform = manifest.platforms[id];
        return platform.visible;
    });
}

export function getBundlesForPlatform(platform) {
    const manifest = getManifest();
    return manifest.platforms[platform].bundles;
}

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

export function selectPackage(packageId, checked, dirty) {
    const pkg = getPackageById(packageId);
    pkg.checked = checked;
    if (!dirty) rerender();
}

export function selectPackages(packages) {
    const manifest = getManifest();
    for (const pkg of Object.values(manifest.packages)) {
        pkg.checked = packages.includes(pkg.id);
    }
    rerender();
}

export function forceShow(id) {
    packageVisibilityOverrides[id] = true;
    rerender();
}

export function getValidatedPackageList(packages) {
    const manifest = getManifest(); 
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

export async function requestBundle(platform) {
    const manifest = getManifest();
    let packages;

    bundlerProgress = {
        progress: 0,
        total: 1,
        currentTask: "Asking the server to create your bundle.",
        isComplete: false,
        success: false
    }
    isBundlingInProgress = true;
    rerender();

    packages = Object.values(manifest.packages).filter((pkg) => {
        return pkg.checked && pkg.platform == platform;
    });

    packages = getValidatedPackageList(packages);
    
    bundlerUuid = await postAndFetchString(ENDPOINT_REQUEST_BUNDLE, {packageset: latestPackageset, packages: packages})

    checkProgressUntilComplete();
}

async function checkProgressUntilComplete() {
    bundlerProgress = await fetchJson(ENDPOINT_BUNDLE_PROGRESS.replace("{id}", bundlerUuid));
    console.log(bundlerProgress);
    modalRerender();

    if (bundlerProgress.isComplete) {
        downloadCompletedBundle();
    } else {
        setTimeout(() => { checkProgressUntilComplete(); }, 200);
    }
}

async function downloadCompletedBundle() {
    window.open(ENDPOINT_DOWNLOAD_BUNDLE.replace("{id}", bundlerUuid));
    isBundlingInProgress = false;
    rerender();
}

export function isBundling() {
    return isBundlingInProgress;
}

export function getBundlerProgress() {
    return bundlerProgress;
}