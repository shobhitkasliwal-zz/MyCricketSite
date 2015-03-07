


function getSiteUrl() {
    // return location.protocol + '//' + location.hostname + (location.port ? ':' + location.port + "/" : '/');

    var loc = document.location.toString();
    if (loc.toLowerCase().indexOf('localhost:') > -1)
        //return location.protocol + '//' + location.hostname + (location.port ? ':' + location.port + "/" : '/');
        return location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '/');
    else {
        var applicationNameIndex = loc.indexOf('/', loc.indexOf('://') + 3);
        var applicationName = loc.substring(0, applicationNameIndex) + '/';
        // var webFolderIndex = _location.indexOf('/', _location.indexOf(applicationName) + applicationName.length);
        /// var webFolderFullPath = _location.substring(0, webFolderIndex) + '/';
        //    return webFolderFullPath;
        return applicationName;
    }
}

function OpenPopup(popupsrc) {
    return $.magnificPopup.open({
        items: {
            src: popupsrc, // can be a HTML string, jQuery object, or CSS selector
            type: 'inline'
        },
        fixedContentPos: false,
        fixedBgPos: true,
        overflowY: 'auto',
        closeBtnInside: true,
        preloader: false,
        midClick: true,
        removalDelay: 300,
        mainClass: 'my-mfp-zoom-in',

    });
}