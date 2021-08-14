import React from 'react';

import * as utils from '../utils';

const LIGHT_THEME = "https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css";
const DARK_THEME = "https://cdn.jsdelivr.net/npm/@forevolve/bootstrap-dark@1.0.0/dist/css/bootstrap-dark.min.css";

export default class ThemeSelector extends React.Component {
    constructor(props) {
        super(props);

        let currentTheme = localStorage.getItem('theme');
        if (!currentTheme || (currentTheme !== "light" && currentTheme !== "dark")) {
            currentTheme = "light";
        }

        let themeLink = document.createElement('link');
        themeLink.id = "theme-link";
        themeLink.rel = "stylesheet";
        themeLink.type = "text/css";
        document.querySelector("head").prepend(themeLink);

        this.selectTheme(currentTheme);

        utils.setThemeSelectHandler((str) => { this.selectTheme(str); });
    }

    selectTheme(str) {
        let link;
        localStorage.setItem('theme', str);

        link = document.querySelector("#theme-link");
        if (str === "dark") {
            link.href = DARK_THEME;
        } else {
            link.href = LIGHT_THEME;
        }
    }

    render() {
        return (
            <></>
        );
    }
}