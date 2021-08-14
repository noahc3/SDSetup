import query from 'query-string'

let themeSelectHandler;

function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
        console.error('Fallback: Oops, unable to copy', err);
    }

    document.body.removeChild(textArea);
}

export function copyTextToClipboard(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).then(function() {
        console.log('Async: Copying to clipboard was successful!');
    }, function(err) {
        console.error('Async: Could not copy text: ', err);
    });
}

export function getPackagesFromQuery() {
    const parsed = query.parse(window.location.search, {arrayFormat: 'comma'});
    if (parsed.packages && parsed.packages.push) {
        return parsed.packages;
    }

    return [];
}

export function buildShareString(platform, packages) {
    let str = query.stringify({packages: packages}, {arrayFormat: 'comma'});
    return window.location.origin + "/share/" + platform + "?" + str;
}

export function setTheme(str) {
    if (typeof(themeSelectHandler) === 'function') themeSelectHandler(str);
}

export function setThemeSelectHandler(func) {
    themeSelectHandler = func;
}