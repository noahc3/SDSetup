from abc import ABC, abstractmethod
import collections
from datetime import datetime
import json
import shutil
from github import Github
from pathlib import Path
import re
import uuid
import zipfile
import os
import config
import urllib.request

from objects import Version1

g = Github(config.access_token)

class PackageFetch(ABC):
    @abstractmethod
    def generateLatestTags(self):
        pass

    @abstractmethod
    def hasUpdate(self):
        pass

    @abstractmethod
    def fetchRelease(self, packageName):
        pass

    @abstractmethod
    def buildValidationData(self):
        pass

    @abstractmethod
    def validate(self, expected, actual):
        pass

    @abstractmethod
    def writeVersionData(self):
        pass

def readJson(path):
    with open(path) as f:
        return json.load(f)

def writeJson(path, obj):
    with open(path, 'w') as f:
        if hasattr(obj, '__dict__'):
            json.dump(obj.__dict__, f, indent=4)
        else:
            json.dump(obj, f, indent=4)
        

def generateTempPath():
    return Path.cwd().joinpath('tmp', str(uuid.uuid4()))

def downloadAsset(asset, path):
    urllib.request.urlretrieve(asset.browser_download_url, path)

def extractZip(file, path):
    with zipfile.ZipFile(file, 'r') as zip_ref:
        zip_ref.extractall(path)

def getAssetByPattern(release, pattern):
    matched_asset = None
    for asset in release.get_assets():
        print(f"        Resolved asset: {asset.name}")
        if re.search(pattern, asset.name):
            matched_asset = asset
            break

    return matched_asset


def getLatestRelease(repo):
    release = g.get_repo(repo).get_latest_release()
    return release

def standardGetCanonicalTag(repo):
    release = getLatestRelease(repo)
    return release.tag_name

def standardHasUpdate(oldTag, newTag):
    return oldTag != newTag

def standardNroFetch(targetDirectory, repo):
    release = getLatestRelease(repo)
    asset = getAssetByPattern(release, ".*\.nro")
    downloadAsset(asset, targetDirectory)

def standardZipFetchRootOnNro(repo, pattern, subdir = "switch"):
    release = getLatestRelease(repo)
    asset = getAssetByPattern(release, pattern)

    tmp = Path.cwd().joinpath("tmp")
    targetDirectory = Path.cwd().joinpath("dist")
    nroRoot = targetDirectory.joinpath(subdir)
    dlPath = tmp.joinpath(asset.name)
    tmp.mkdir(parents=True, exist_ok=True)
    nroRoot.mkdir(parents=True, exist_ok=True)
    
    downloadAsset(asset, dlPath)
    extractZip(dlPath, tmp)

    dlPath.unlink()
    
    for root, dirs, files in os.walk(tmp):
        for file in files:
            if file.endswith(".nro"):
                os.replace(os.path.join(root, file), nroRoot.joinpath(file))

    shutil.rmtree(tmp, ignore_errors=True)

def standardBuildValidationData():
    dirs = set()
    files = set()

    dist = Path.cwd().joinpath("dist")

    for root, dir, file in os.walk(dist):
        for d in dir:
            dirs.add(str(Path(root).joinpath(d).relative_to(dist)))
        for f in file:
            files.add(str(Path(root).joinpath(f).relative_to(dist)))

    return {'dirs': list(dirs), 'files': list(files)}

def standardValidate(expected, actual):
    return collections.Counter(expected['files']) == collections.Counter(actual['files']) and collections.Counter(expected['dirs']) == collections.Counter(actual['dirs'])

def standardWriteVersionData(canonicalTag, displayTag):
    ver = Version1()
    ver.canonicalTag = canonicalTag
    ver.displayTag = displayTag
    ver.timestamp = datetime.utcnow().isoformat()
    ver.size = 0

    writeJson(Path.cwd().joinpath("version.json"), ver)