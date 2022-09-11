#!/usr/bin/env python3
import json
import utils
from utils import PackageFetch

class Fetch(PackageFetch):
    repo = "switchbrew/nx-hbmenu"
    canonicalTag = "v0.0.0"
    displayTag = "v0.0.0"

    def __init__(self):
        pass

    def getCanonicalTag(self):
        canonicalTag = utils.standardGetCanonicalTag(self.repo)
        return canonicalTag

    def hasUpdate(self):
        return utils.standardHasUpdate(self.repo, self.canonicalTag)

    def fetchRelease(self):
        utils.standardZipFetchRootOnNro(self.repo, "nx-hbmenu.*\.zip")
        
    def buildValidationData(self):
        return utils.standardBuildValidationData()

    def validate(self, expected, actual):
        return utils.standardValidate(expected, actual)