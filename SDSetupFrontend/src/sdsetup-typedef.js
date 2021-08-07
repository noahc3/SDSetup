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
 * }} BundlerProgress
 * 
 */