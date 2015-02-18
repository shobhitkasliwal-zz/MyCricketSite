


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