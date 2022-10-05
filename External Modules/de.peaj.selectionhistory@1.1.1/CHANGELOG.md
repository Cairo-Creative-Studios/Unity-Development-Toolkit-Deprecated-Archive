# Changelog
All notable changes to this package are documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.1.1] - 2021-02-01
### Fixed
- Updated package.json to have correct version number

## [1.1.0] - 2021-02-01

This release greatly enhances the usability of the Selection History Window.
History entries can now be directly selected (via click) and even dragged out of the history to drop them anywhere in the Unity editor.
Also the selection history now survives an assembly reload after recompiling.

![Selection History Window](https://raw.githubusercontent.com/Peaj/Unitility/main/Packages/de.peaj.selectionhistory/Documentation%7E/Images/Screenshot%202021-02-01%20004618.png)

### Changed
- Decreased Unity version requirement to 2019.3.5f1

### Added
- Enhance selection history window
  - Selectable history entries
  - Dragable history entries
  - Multi selection object counter
  - Auto scroll to current history entry
  - Clear button in context menu
- Recovery from assembly reload
- Clear deleted objects from history

### Fixed
- Fix last object being skipped after deselection
- Fix Windows specific functionality not excluded on Mac/Linux
- Fix back/forward mouse buttons being registered when Unity is not focused

## [1.0.0] - 2021-01-29

This is the first release of Selection History.

### Added
- Back functionality
- Forward functionality
- Keyboard shortcuts
- Mouse button support
- Selection history window
