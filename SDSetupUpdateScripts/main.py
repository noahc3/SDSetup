#!/usr/bin/env python3

from email import utils
import importlib.util
import shutil
from pathlib import Path
import os

import utils
from utils import PackageFetch

def loadModule(name, path):
    spec = importlib.util.spec_from_file_location(name, path)
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    return module.Fetch()

def findPackages() -> "dict(str, PackageFetch)":
    packages: "dict(str, PackageFetch)" = {}
    for root, dirs, files in os.walk("packages"):
        for file in files:
            if file == "fetch.py":
                name = root.split("/")[1]
                path = os.path.join(root, file)
                packages[name] = loadModule(name, path)

    return packages

def main():
    root = Path.cwd().absolute()
    print(f"Loaded in {root}")

    shutil.rmtree("tmp", ignore_errors=True)
    shutil.rmtree("out", ignore_errors=True)
    os.mkdir("out")

    packages: "dict(str, PackageFetch)" = findPackages()

    for package in packages:
        print(f"Fetching {package}")
        module: PackageFetch = packages[package]
        canonicalTag = module.getCanonicalTag()
        print(f"    Canonical tag: {canonicalTag}")
        hasUpdate = module.hasUpdate()
        print(f"    Has update: {hasUpdate}")
        if hasUpdate:
            outdir = root.joinpath("out", package, canonicalTag)
            print(f"    Outputting to {outdir}")
            outdir.mkdir(parents=True, exist_ok=True)
            os.chdir(outdir)
            print(f"    Performing fetch")
            module.fetchRelease()

            validationDataPath = root.joinpath("packages", package, "validation.json")
            actual = module.buildValidationData()
            if validationDataPath.exists():
                expected = utils.readJson(validationDataPath)
                valid = module.validate(expected, actual)
                if (valid):
                    print("    Validation passed")
                else:
                    print("    Validation failed")
                    print(f"       Expected: {expected}")
                    print(f"       Actual: {actual}")
                    print("    Skipping update, please verify the fetch script is up-to-date with the latest release format")
                    shutil.rmtree(outdir, ignore_errors=True)
            else:
                print("    No validation data found, generating validation data")
                utils.writeJson(validationDataPath, actual)
main()