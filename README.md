<p align="center">
    <a href="https://preview.sdsetup.com">
        <img alt="sdsetup" src="https://www.sdsetup.com/img/logo.png">
    </a>
</p>

<p align="center">
    <img alt="License: AGPLv3" src="https://img.shields.io/badge/License-AGPL%20v3-blue.svg"/>
    <img alt="Build Status" src="https://github.com/noahc3/SDSetupPreview/actions/workflows/build-and-deploy.yml/badge.svg?branch=rewrite"/>
</p>

SDSetup is a web application written in JavaScript and C#. The site lets you select from a list of homebrew applications and custom firmwares to quickly create a zip archive to extract to your SD card. This is effectively the Ninite for your game consoles.

## Features
### General
* Generate a perfectly formatted file structure in ZIP format, ready for direct extraction to your SD card. No additional setup necessary, just drag and drop!
* Save your setup to reselect packages later, or share with a friend.
* Don't want to customize? Quick-download from a list of pre-configured bundles.

### Nintendo Switch
* Choose between a selection of common homebrew applications, tools and utilities, including:
	* Custom Firmwares (ex. Atmosphere)
    * CFW Options (ex. Sysmodules, Tesla plugins)
	* Utilities (ex. Homebrew Menu, Checkpoint, JKSV)
	* Emulators (ex. Salamander RetroNX, pSNES)
	* Fusee Payloads (ex. Hekate, BISKeyDump, BriccMii)
    * PegaScape Paylods (ex. Caffeine, Nereba)
	* PC Tools (ex. TegraRcmGui)
    * Android Tools (ex. Rekado)

## Usage
Head over to [https://preview.sdsetup.com](https://preview.sdsetup.com), select your console of choice, select the packages you want, and hit "Get My Bundle"! Once finished, simply **extract the contents of the sd folder** in the downloaded ZIP archive **to the root of your SD card!** Do what you wish with any additional folders included in the zip file.

## Compatibility
The user-facing frontend is written in JS with React and should be functional on most browsers. 

The admin control panel is written in C# with Blazor and requries a WASM capable browser to function.

## Issues
Please feel free [submit an issue](https://www.github.com/noahc3/sdsetup/issues) for any of the following reasons:
* A package is outdated
* A package's information is incorrect
* A package should be retrieved from a different/better source
* A browser other than Internet Explorer is incompatible
* Reporting a bug
* Suggesting a feature
* Suggesting a new package
* Requesting a package be removed Homebrew SD setup
* Reporting a redistribution clause license violation for a rehosted package
* Any other issue with the site or it's packages

## Build
The backend server, manager and common library can be built easily by opening the project solution in Visual Studio 2022.

The frontend is a Node.js project written in React, launch into debug mode with the command `npm start`. You can build the static output with `npm run build`.

## Included Projects
* **SDSetup Frontend:** The user-facing frontend, written in JavaScript with React.
* **SDSetup Backend:** The backend server which the frontend communicates with. Upon request, generates zip bundles based on the client selection. Also provides a management API for use with SDSetup Manager. Written in C# with ASP.NET Core.
* **SDSetup Manager:** Control panel to manage functionality of the backend server, including package data, auto-update configuration, error tracking and more. Written in C# with Blazor.
* **SDSetup Common:** Common utilities and data types shared between the backend and the manager.

## Todo
* See [this issue](https://github.com/noahc3/SDSetup/issues/203) for a list of features still planned or in-progress.

## Contributing
Feel free to make pull requests where you see fit!

## Credits
Please see https://preview.sdsetup.com/credits for an up-to-date list of credits and sources for each package available.

Other credits:
* Team AtlasNX for originally working with me to get SDSetup off the ground.
* Team Neptune for providing amazing user support for SDSetup bundles.
* Behemoth, hax4dayz, Slluxx, PanSaborATrapo, Austin, and TechGeekGamer for helping me out a ton during the transition period.