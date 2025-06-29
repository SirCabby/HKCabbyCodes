# Makefile for CabbyCodes - Hollow Knight Mod
# This Makefile provides common build operations for the CabbyCodes project

# Configuration
PROJECT_NAME = CabbyCodes
SOLUTION_FILE = CabbyCodes.sln
PROJECT_FILE = CabbyCodes/CabbyCodes.csproj
TARGET_FRAMEWORK = net472
CONFIGURATION = Release
PLATFORM = Any CPU

# Project-specific paths
CABBYCODES_PROJECT = CabbyCodes/CabbyCodes.csproj
CABBYMENU_PROJECT = CabbyMenu/CabbyMenu.csproj

# Paths
BUILD_DIR = CabbyCodes/bin/$(CONFIGURATION)/$(TARGET_FRAMEWORK)
OBJ_DIR = CabbyCodes/obj/$(CONFIGURATION)/$(TARGET_FRAMEWORK)
CABBYMENU_BUILD_DIR = CabbyMenu/bin/$(CONFIGURATION)/$(TARGET_FRAMEWORK)
CABBYMENU_OBJ_DIR = CabbyMenu/obj/$(CONFIGURATION)/$(TARGET_FRAMEWORK)
OUTPUT_DIR = $(BUILD_DIR)
DLL_NAME = $(PROJECT_NAME).dll

# Hollow Knight installation path (modify as needed)
HOLLOW_KNIGHT_PATH = "C:\Program Files (x86)\Steam\steamapps\common\Hollow Knight"
BEPINEX_PLUGINS_PATH = $(HOLLOW_KNIGHT_PATH)\BepInEx\plugins

# .NET SDK version (from global.json)
DOTNET_VERSION = 6.0.318

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
	rm -rf $(BUILD_DIR)
	rm -rf $(OBJ_DIR)
	rm -rf $(CABBYMENU_BUILD_DIR)
	rm -rf $(CABBYMENU_OBJ_DIR)
	@echo "Clean completed!"

# Deep clean (removes all build artifacts and temporary files)
.PHONY: clean-all
clean-all: clean
	@echo "Performing deep clean..."
	rm -rf CabbyCodes/bin/
	rm -rf CabbyCodes/obj/
	rm -rf CabbyMenu/bin/
	rm -rf CabbyMenu/obj/
	@echo "Deep clean completed!"

# Clean individual projects
.PHONY: clean-cabbycodes
clean-cabbycodes:
	@echo "Cleaning CabbyCodes project..."
	dotnet clean $(CABBYCODES_PROJECT)
	rm -rf $(BUILD_DIR)
	rm -rf $(OBJ_DIR)
	@echo "CabbyCodes clean completed!"

.PHONY: clean-cabbymenu
clean-cabbymenu:
	@echo "Cleaning CabbyMenu project..."
	dotnet clean $(CABBYMENU_PROJECT)
	rm -rf $(CABBYMENU_BUILD_DIR)
	rm -rf $(CABBYMENU_OBJ_DIR)
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
	dotnet publish $(PROJECT_FILE) --configuration $(CONFIGURATION) --output $(OUTPUT_DIR)
	@echo "Publish completed!"

# Deploy to Hollow Knight (copy DLL to BepInEx plugins folder)
.PHONY: deploy
deploy: build
	@echo "Deploying $(PROJECT_NAME) to Hollow Knight..."
	@powershell -Command "\
if (Test-Path '$(BEPINEX_PLUGINS_PATH)') { \
    Copy-Item '$(OUTPUT_DIR)/$(DLL_NAME)' '$(BEPINEX_PLUGINS_PATH)/' -Force; \
    Copy-Item '$(CABBYMENU_BUILD_DIR)/CabbyMenu.dll' '$(BEPINEX_PLUGINS_PATH)/' -Force; \
    Write-Host 'Deployed CabbyCodes.dll and CabbyMenu.dll to $(BEPINEX_PLUGINS_PATH)'; \
} else { \
    Write-Host 'Error: BepInEx plugins directory not found at $(BEPINEX_PLUGINS_PATH)'; \
    Write-Host 'Please ensure Hollow Knight is installed and BepInEx is set up correctly.'; \
    exit 1; \
}"

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
	@echo "Output DLL: $(OUTPUT_DIR)/$(DLL_NAME)"

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
	@echo "  check-sdk        - Check .NET SDK version"
	@echo "  info             - Show project information"
	@echo "  help             - Show this help message"

# Package the mod (create a zip file for distribution)
.PHONY: package
package: build
	@echo "Packaging $(PROJECT_NAME)..."
	@mkdir -p dist
	@cp "$(OUTPUT_DIR)/$(DLL_NAME)" dist/
	@if [ -f README.md ]; then cp README.md dist/; fi
	@cd dist && zip -r "../$(PROJECT_NAME)-$(shell date +%Y%m%d).zip" .
	@echo "Package created: $(PROJECT_NAME)-$(shell date +%Y%m%d).zip"
