from pickletools import long1


class Package6:
    id: str
    shortName: str
    name: str
    description: str
    authors: str
    source: str
    license: str
    downloads: int

    platform: str
    section: str
    category: str
    subcategory: str
    
    visible: bool
    showsInCredits: bool
    enabledByDefault: bool

    priority: int
    dependencyMode: int
    dependencies: "list(str)"
    autoDependencies: "list(str)"

    @staticmethod
    def fromDict(data: "dict(str, any)") -> "Package6":
        package = Package6()
        package.id = data["id"]
        package.shortName = data["shortName"]
        package.name = data["name"]
        package.description = data["description"]
        package.authors = data["authors"]
        package.source = data["source"]
        package.license = data["license"]
        package.downloads = data["downloads"]

        package.platform = data["platform"]
        package.section = data["section"]
        package.category = data["category"]
        package.subcategory = data["subcategory"]

        package.visible = data["visible"]
        package.showsInCredits = data["showsInCredits"]
        package.enabledByDefault = data["enabledByDefault"]

        package.priority = data["priority"]
        package.dependencyMode = data["dependencyMode"]
        package.dependencies = data["dependencies"]
        package.autoDependencies = data["autoDependencies"]

        return package

class Version1:
    displayTag: str
    canonicalTag: str
    fetchDate: str
    size: int

    @staticmethod
    def fromDict(data: "dict(str, any)") -> "Version1":
        version = Version1()
        version.displayTag = data["displayTag"]
        version.canonicalTag = data["canonicalTag"]
        version.fetchDate = data["fetchDate"]
        version.size = data["size"]

        return version