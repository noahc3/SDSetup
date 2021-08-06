import { ThemeProvider } from 'react-bootstrap';

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
 * @param {string} id 
 */
export function forceShow(id) {
    packageVisibilityOverrides[id] = true;
    rerender();
}

/**
 * 
 * @param {string[]} packages 
 * @returns {string[]}
 */
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

/**
 * 
 * @param {string} platform 
 */
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
 * Manifest
 * @typedef {{
 * packageset: string,
 * version: string,
 * copyright: string,
 * message: Message,
 * platforms: Object.<string, Platform>,
 * packages: Object.<string, Package>
 * }} Manifest
 * 
 * Package
 * @typedef {{
 * id: string,
 * name: string,
 * displayName: string,
 * plaform: Platform,
 * section: Section,
 * category: Category,
 * subcategory: Subcategory,
 * authors: string,
 * downloads: number,
 * versionInfo: VersionInfo,
 * source: string,
 * license: string,
 * priority: number,
 * enabledByDefault: boolean,
 * visible: boolean,
 * showsInCredits: boolean,
 * description: string,
 * when: string[],
 * whenMode: number,
 * warning: Warning,
 * dependencies: string[],
 * deleteOnUpdate: string[],
 * autoUpdates: boolean
 * }} Package
 * 
 * VersionInfo
 * @typedef {{
 * version: string,
 * size: number
 * }} VersionInfo
 * 
 * Platform
 * @typedef {{
 * name: string,
 * menuName: string,
 * homeIcon: string,
 * id: string,
 * color: string,
 * visible: boolean,
 * packageSections: Object.<string, Section>,
 * bundles: Bundle[]
 * }} Platform
 * 
 * Section
 * @typedef {{
 * id: string,
 * name: string,
 * displayName: string,
 * listingMode: number,
 * visible: boolean,
 * when: string[],
 * whenMode: number,
 * footer: string,
 * categories: Object.<string, Category>
 * }} Section
 * 
 * Category
 * @typedef {{
 * id: string,
 * name: string,
 * displayName: string,
 * visible: boolean,
 * when: string[],
 * whenMode: number,
 * subcategories: Object.<string, Subcategory>
 * }} Category
 * 
 * Subcategory
 * @typedef {{
 * id: string,
 * name: string,
 * displayName: string,
 * visible: boolean,
 * when: string[],
 * whenMode: number,
 * packages: string[]
 * }} Subcategory
 * 
 * Bundle
 * @typedef {{
 * name: string,
 * description: string,
 * packages: string[]
 * }} Bundle
 * 
 * Warning
 * @typedef {{
 * title: string,
 * content: string,
 * packageID: string
 * }} Warning
 * 
 * Message
 * @typedef {{
 * color: string,
 * innerHTML: string
 * }} Message
 * 
 * BundlerProgress
 * @typedef {{
 * progress: number,
 * total: number,
 * currentTask: string,
 * isComplete: boolean,
 * success: boolean,
 * completionTime: string
 * }}
 */