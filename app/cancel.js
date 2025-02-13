$(() => {
    const remote = require('electron').remote;
    const app = remote.app;
    const fs = require('fs');
    const path = require('path');
    var rimraf = require("rimraf");
    const shell = require('electron').shell;
    const win = remote.getCurrentWindow();
    const configPath = path.join(app.getPath('userData'), 'config.json');
    let installerPath = 'none';
    const versionInformationURL = 'https://d76a05d74f889aafd38d-39162a6e09ffdab7394e3243fa2342c1.ssl.cf2.rackcdn.com/version.json';

    function getDownloadStoragePath() {
        return JSON.parse(fs.readFileSync(configPath)).downloadStoragePath;
    }

    function onMinimizeButtonPress() {
        win.minimize();
    }

    function onWebsiteLinkPress() {
        shell.openExternal('https://www.realitymod.com')
    }

    function onGithubLinkPress() {
        shell.openExternal('https://github.com/WouterJansen/PRBF2Download')
    }

    function onCloseButtonPress() {
        win.close();
    }

    function getTorrentFolderName(handleData) {
        $.ajax({
            url: versionInformationURL,
            success:function(data) {
                handleData(data.torrent_foldername);
            }
        });
    }

    function onContinueButtonPress() {
        win.loadFile('./app/download.html')
    }

    function onCancelButtonPress() {
        win.close();
    }

    function onCancelAndRemoveButtonPress() {
        try{
            rimraf.sync(installerPath);
            rimraf.sync(path.join(app.getPath('userData'), 'TorrentFiles',"*"));
        }catch(err){
            console.log(err)
        }
        fs.unlinkSync(configPath);
        win.close();
    }

    getTorrentFolderName(function(torrent_foldername){
        installerPath = path.join(getDownloadStoragePath(), torrent_foldername);
    });

    document.querySelector('#cancel-button').addEventListener('click', onCancelButtonPress);
    document.querySelector('#cancel-remove-button').addEventListener('click', onCancelAndRemoveButtonPress);
    document.querySelector('#continue-button').addEventListener('click', onContinueButtonPress);
    document.querySelector('#close-button').addEventListener('click', onCloseButtonPress);
    document.querySelector('#minimize-button').addEventListener('click', onMinimizeButtonPress);
    document.querySelector('#website-link').addEventListener('click', onWebsiteLinkPress);
    document.querySelector('#github-link').addEventListener('click', onGithubLinkPress);
});