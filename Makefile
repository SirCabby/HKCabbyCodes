# Makefile for CabbyCodes - Hollow Knight Mod
# This Makefile provides common build operations for the CabbyCodes project

# Configuration
PROJECT_NAME = CabbyCodes
SOLUTION_FILE = CabbyCodes.sln
TARGET_FRAMEWORK = net472
CONFIGURATION = Release
PLATFORM = Any CPU

# Project-specific paths
CABBYCODES_PROJECT = CabbyCodes/CabbyCodes.csproj
CABBYMENU_PROJECT = CabbyMenu/CabbyMenu.csproj

# Paths
BUILD_DIR = CabbyCodes\bin\$(CONFIGURATION)\$(TARGET_FRAMEWORK)
OBJ_DIR = CabbyCodes\obj\$(CONFIGURATION)\$(TARGET_FRAMEWORK)
CABBYMENU_BUILD_DIR = CabbyMenu\bin\$(CONFIGURATION)\$(TARGET_FRAMEWORK)
CABBYMENU_OBJ_DIR = CabbyMenu\obj\$(CONFIGURATION)\$(TARGET_FRAMEWORK)

# Hollow Knight installation path (modify as needed)
HOLLOW_KNIGHT_PATH = C:\Program Files (x86)\Steam\steamapps\common\Hollow Knight
BEPINEX_PLUGINS_PATH = $(HOLLOW_KNIGHT_PATH)\BepInEx\plugins

# .NET SDK version (from global.json)
DOTNET_VERSION = 9.0.301

# Default target
.PHONY: all
all: build

# Build the project
.PHONY: build
build: restore 
	@echo "Building $(PROJECT_NAME) solution..."
	dotnet build $(SOLUTION_FILE) --configuration $(CONFIGURATION) --no-restore
	@echo "Build completed successfully!"

# Build in Debug configuration
.PHONY: debug
debug:
	@echo "Building $(PROJECT_NAME) solution in Debug configuration..."
	dotnet build $(SOLUTION_FILE) --configuration Debug --no-restore
	@echo "Debug build completed successfully!"

# Build individual projects
.PHONY: build-cabbycodes
build-cabbycodes: restore
	@echo "Building CabbyCodes project..."
	dotnet build $(CABBYCODES_PROJECT) --configuration $(CONFIGURATION) --no-restore
	@echo "CabbyCodes build completed!"

.PHONY: build-cabbymenu
build-cabbymenu: restore
	@echo "Building CabbyMenu project..."
	dotnet build $(CABBYMENU_PROJECT) --configuration $(CONFIGURATION) --no-restore
	@echo "CabbyMenu build completed!"

# Restore NuGet packages
.PHONY: restore
restore:
	@echo "Restoring NuGet packages for solution..."
	dotnet restore $(SOLUTION_FILE)
	@echo "Package restore completed!"

# Clean build artifacts
.PHONY: clean
clean:
	@echo "Cleaning build artifacts..."
	dotnet clean $(SOLUTION_FILE)
	@if exist $(BUILD_DIR) rmdir /s /q $(BUILD_DIR)
	@if exist $(OBJ_DIR) rmdir /s /q $(OBJ_DIR)
	@if exist $(CABBYMENU_BUILD_DIR) rmdir /s /q $(CABBYMENU_BUILD_DIR)
	@if exist $(CABBYMENU_OBJ_DIR) rmdir /s /q $(CABBYMENU_OBJ_DIR)
	@echo "Clean completed!"

# Deep clean (removes all build artifacts and temporary files)
.PHONY: clean-all
clean-all: clean
	@echo "Performing deep clean..."
	@if exist CabbyCodes\bin rmdir /s /q CabbyCodes\bin
	@if exist CabbyCodes\obj rmdir /s /q CabbyCodes\obj
	@if exist CabbyMenu\bin rmdir /s /q CabbyMenu\bin
	@if exist CabbyMenu\obj rmdir /s /q CabbyMenu\obj
	@echo "Deep clean completed!"

# Clean individual projects
.PHONY: clean-cabbycodes
clean-cabbycodes:
	@echo "Cleaning CabbyCodes project..."
	dotnet clean $(CABBYCODES_PROJECT)
	@if exist $(BUILD_DIR) rmdir /s /q $(BUILD_DIR)
	@if exist $(OBJ_DIR) rmdir /s /q $(OBJ_DIR)
	@echo "CabbyCodes clean completed!"

.PHONY: clean-cabbymenu
clean-cabbymenu:
	@echo "Cleaning CabbyMenu project..."
	dotnet clean $(CABBYMENU_PROJECT)
	@if exist $(CABBYMENU_BUILD_DIR) rmdir /s /q $(CABBYMENU_BUILD_DIR)
	@if exist $(CABBYMENU_OBJ_DIR) rmdir /s /q $(CABBYMENU_OBJ_DIR)
	@echo "CabbyMenu clean completed!"

# Run tests (if any)
.PHONY: test
test:
	@echo "Running tests..."
	dotnet test $(SOLUTION_FILE) --configuration $(CONFIGURATION) --no-build
	@echo "Tests completed!"

# Publish the project
.PHONY: publish
publish: build
	@echo "Publishing $(PROJECT_NAME)..."
	dotnet publish $(CABBYCODES_PROJECT) --configuration $(CONFIGURATION) --output $(BUILD_DIR)
	@echo "Publish completed!"

# Deploy to Hollow Knight (copy DLL to BepInEx plugins folder)
.PHONY: deploy
deploy: build
	@echo Closing any existing Hollow Knight processes...
	@taskkill /f /im "hollow_knight.exe" >nul 2>&1 || exit /b 0
	@echo Deploying $(PROJECT_NAME) to Hollow Knight...
	@if not exist "$(BUILD_DIR)" echo Error: Output directory "$(BUILD_DIR)" does not exist. && exit /b 1
	@if not exist "$(CABBYMENU_BUILD_DIR)" echo Error: CabbyMenu output directory "$(CABBYMENU_BUILD_DIR)" does not exist. && exit /b 1
	@if not exist "$(BEPINEX_PLUGINS_PATH)" echo Error: BepInEx plugins directory "$(BEPINEX_PLUGINS_PATH)" not found. Please ensure Hollow Knight is installed and BepInEx is set up correctly. && exit /b 1
	@if not exist "$(BUILD_DIR)\$(PROJECT_NAME).dll" echo Error: $(PROJECT_NAME).dll not found at "$(BUILD_DIR)\$(PROJECT_NAME).dll" && exit /b 1
	@if not exist "$(CABBYMENU_BUILD_DIR)\CabbyMenu.dll" echo Error: CabbyMenu.dll not found at "$(CABBYMENU_BUILD_DIR)\CabbyMenu.dll" && exit /b 1
	@if exist "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" del /f /q "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll"
	@if exist "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" del /f /q "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll"
	@if exist "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" ( echo Error: $(PROJECT_NAME).dll still exists after delete! && exit /b 1 )
	@if exist "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" ( echo Error: CabbyMenu.dll still exists after delete! && exit /b 1 )
	@copy /Y "$(BUILD_DIR)\$(PROJECT_NAME).dll" "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll"
	@copy /Y "$(CABBYMENU_BUILD_DIR)\CabbyMenu.dll" "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll"
	@if exist "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" ( echo $(PROJECT_NAME).dll successfully copied to plugins folder. ) else ( echo Error: $(PROJECT_NAME).dll was not copied! )
	@if exist "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" ( echo CabbyMenu.dll successfully copied to plugins folder. ) else ( echo Error: CabbyMenu.dll was not copied! )
	@echo Deploy complete.

# Deploy and run Hollow Knight
.PHONY: run
run: deploy
	@echo Launching Hollow Knight via Steam...
	@start "" "steam://rungameid/367520"
	@echo Hollow Knight launched via Steam!

# Check .NET SDK version
.PHONY: check-sdk
check-sdk:
	@echo "Checking .NET SDK version..."
	@dotnet --version
	@echo "Required version: $(DOTNET_VERSION)"

# Show project information
.PHONY: info
info:
	@echo "Solution: $(SOLUTION_FILE)"
	@echo "Main Project: $(PROJECT_NAME)"
	@echo "Target Framework: $(TARGET_FRAMEWORK)"
	@echo "Configuration: $(CONFIGURATION)"
	@echo "Platform: $(PLATFORM)"
	@echo "Projects:"
	@echo "  - CabbyCodes: $(CABBYCODES_PROJECT)"
	@echo "  - CabbyMenu: $(CABBYMENU_PROJECT)"
	@echo "Build Directory: $(BUILD_DIR)"
	@echo "Output DLL: $(BUILD_DIR)/$(PROJECT_NAME).dll"

# Help target
.PHONY: help
help:
	@echo "Available targets:"
	@echo "  all              - Build the solution (default)"
	@echo "  build            - Build the solution"
	@echo "  debug            - Build in Debug configuration"
	@echo "  build-cabbycodes - Build only CabbyCodes project"
	@echo "  build-cabbymenu  - Build only CabbyMenu project"
	@echo "  restore          - Restore NuGet packages"
	@echo "  clean            - Clean build artifacts"
	@echo "  clean-all        - Deep clean (removes all build artifacts)"
	@echo "  clean-cabbycodes - Clean only CabbyCodes project"
	@echo "  clean-cabbymenu  - Clean only CabbyMenu project"
	@echo "  test             - Run tests"
	@echo "  publish          - Publish the project"
	@echo "  deploy           - Deploy to Hollow Knight BepInEx plugins folder"
	@echo "  run              - Deploy and run Hollow Knight"
	@echo "  check-sdk        - Check .NET SDK version"
	@echo "  info             - Show project information"
	@echo "  rev x.x.x        - Update version numbers across projects"
	@echo "  package          - Build and create release package using project version"
	@echo "  help             - Show this help message"

# Update version numbers only
.PHONY: rev
rev:
	@if "$(filter-out $@,$(MAKECMDGOALS))" == "" (echo Error: Version parameter is required. Use: make rev x.x.x && exit /b 1)
	@echo "Updating version to $(filter-out $@,$(MAKECMDGOALS))..."
	@powershell -Command "(Get-Content 'CabbyCodes\CabbyCodes.csproj') -replace '<Version>.*</Version>', '<Version>$(filter-out $@,$(MAKECMDGOALS))</Version>' | Set-Content 'CabbyCodes\CabbyCodes.csproj'"
	@powershell -Command "(Get-Content 'CabbyMenu\CabbyMenu.csproj') -replace '<Version>.*</Version>', '<Version>$(filter-out $@,$(MAKECMDGOALS))</Version>' | Set-Content 'CabbyMenu\CabbyMenu.csproj'"
	@powershell -Command "(Get-Content 'CabbyCodes\Constants.cs') -replace 'public const string VERSION = \".*\";', 'public const string VERSION = \"$(filter-out $@,$(MAKECMDGOALS))\";' | Set-Content 'CabbyCodes\Constants.cs'"
	@echo "Version files updated successfully!"

# Catch-all target to prevent make from complaining about unknown targets when using positional args
%:
	@:

# Package the mod (create a zip file for distribution)
.PHONY: package
package: build
	@echo "Packaging $(PROJECT_NAME)..."
	@if not exist Output mkdir Output
	@powershell -NoProfile -Command "$$ver = ([regex]::Match((Get-Content 'CabbyCodes\\CabbyCodes.csproj' -Raw), '<Version>([^<]+)</Version>')).Groups[1].Value; Compress-Archive -Path '$(BUILD_DIR)\\$(PROJECT_NAME).dll','$(CABBYMENU_BUILD_DIR)\\CabbyMenu.dll' -DestinationPath ('Output\\$(PROJECT_NAME)_v' + $$ver + '.zip') -Force"
	@echo "Package created in Output folder."
