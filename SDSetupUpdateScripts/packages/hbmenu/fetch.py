#!/usr/bin/env python3
import utils
import datetime
from utils import PackageFetch
from objects import Version1

class Fetch(PackageFetch):
    repo = "switchbrew/nx-hbmenu"
    canonicalTag = "v0.0.0"
    displayTag = "v0.0.0"

    def __init__(self):
        pass

    def generateLatestTags(self):
        self.canonicalTag = utils.standardGetCanonicalTag(self.repo)
        self.displayTag = self.canonicalTag
        return self.canonicalTag, self.displayTag

    def hasUpdate(self):
        return utils.standardHasUpdate(self.repo, self.canonicalTag)

    def fetchRelease(self):
        utils.standardZipFetchRootOnNro(self.repo, "nx-hbmenu.*\.zip", "")
        
    def buildValidationData(self):
        return utils.standardBuildValidationData()

    def validate(self, expected, actual):
        return utils.standardValidate(expected, actual)

    def writeVersionData(self):
        utils.standardWriteVersionData(self.canonicalTag, self.displayTag) 