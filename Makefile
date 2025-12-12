# Makefile for CabbyCodes - Hollow Knight Mod
# This Makefile provides common build operations for the CabbyCodes project

# Configuration
PROJECT_NAME = CabbyCodes
SOLUTION_FILE = CabbyCodes.sln
TARGET_FRAMEWORK = net472
CONFIGURATION = Release
PLATFORM = Any CPU

# Version parameter (defaults to 6)
VERSION ?= 6

# Capture version from command line arguments for build, deploy, run targets
ifneq (,$(filter build deploy run,$(MAKECMDGOALS)))
    ifneq (,$(firstword $(MAKECMDGOALS)))
        ARG_VERSION := $(word 2,$(MAKECMDGOALS))
        ifneq (,$(ARG_VERSION))
            VERSION := $(ARG_VERSION)
        endif
    endif
endif

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
LUMAFLY_MODS_PATH = $(HOLLOW_KNIGHT_PATH)\hollow_knight_Data\Managed\Mods

# HarmonyX DLL path for Lumafly builds (HKAPI doesn't include Harmony)
HARMONY_DLL = $(USERPROFILE)\.nuget\packages\harmonyx\2.10.2\lib\net45\0Harmony.dll

# .NET SDK version (from global.json)
DOTNET_VERSION = 9.0.301

# Default target
.PHONY: all
all: build

# Build the project (defaults to BepInEx 6)
# Usage: make build [VERSION=5|6]
.PHONY: build
build: 
	@if "$(VERSION)" == "5" ( \
		echo "Restoring NuGet packages for BepInEx 5..." && \
		dotnet restore $(SOLUTION_FILE) /p:Configuration=BepInEx5 && \
		echo "Building $(PROJECT_NAME) solution for BepInEx 5..." && \
		dotnet build $(CABBYCODES_PROJECT) --configuration BepInEx5 --no-restore && \
		dotnet build $(CABBYMENU_PROJECT) --configuration BepInEx5 --no-restore \
	) else ( \
		echo "Restoring NuGet packages for BepInEx 6..." && \
		dotnet restore $(SOLUTION_FILE) /p:Configuration=BepInEx6 && \
		echo "Building $(PROJECT_NAME) solution for BepInEx 6..." && \
		dotnet build $(CABBYCODES_PROJECT) --configuration BepInEx6 --no-restore && \
		dotnet build $(CABBYMENU_PROJECT) --configuration BepInEx6 --no-restore \
	)
	@echo "Build completed successfully!"

# Build in Debug configuration
.PHONY: debug
debug:
	@echo "Building $(PROJECT_NAME) solution in Debug configuration..."
	dotnet build $(SOLUTION_FILE) --configuration Debug --no-restore
	@echo "Debug build completed successfully!"

# Build individual projects (defaults to BepInEx 6)
.PHONY: build-cabbycodes
build-cabbycodes: restore
	@echo "Building CabbyCodes project for BepInEx 6..."
	dotnet build $(CABBYCODES_PROJECT) --configuration BepInEx6 --no-restore
	@echo "CabbyCodes build completed!"

.PHONY: build-cabbymenu
build-cabbymenu: restore
	@echo "Building CabbyMenu project for BepInEx 6..."
	dotnet build $(CABBYMENU_PROJECT) --configuration BepInEx6 --no-restore
	@echo "CabbyMenu build completed!"

# Build for BepInEx 5
.PHONY: build-bepinex5
build-bepinex5:
	@echo "Restoring NuGet packages for BepInEx 5..."
	dotnet restore $(SOLUTION_FILE) /p:Configuration=BepInEx5
	@echo "Building for BepInEx 5..."
	dotnet build $(CABBYCODES_PROJECT) --configuration BepInEx5 --no-restore
	dotnet build $(CABBYMENU_PROJECT) --configuration BepInEx5 --no-restore
	@echo "BepInEx 5 build completed!"

# Build for BepInEx 6
.PHONY: build-bepinex6
build-bepinex6:
	@echo "Restoring NuGet packages for BepInEx 6..."
	dotnet restore $(SOLUTION_FILE) /p:Configuration=BepInEx6
	@echo "Building for BepInEx 6..."
	dotnet build $(CABBYCODES_PROJECT) --configuration BepInEx6 --no-restore
	dotnet build $(CABBYMENU_PROJECT) --configuration BepInEx6 --no-restore
	@echo "BepInEx 6 build completed!"

# Build for Lumafly (HKAPI)
# NOTE: Requires HKAPI-patched Assembly-CSharp.dll in CabbyCodes/lib-lumafly/
.PHONY: build-lumafly
build-lumafly:
	@if not exist "CabbyCodes\lib-lumafly\Assembly-CSharp.dll" ( \
		echo ERROR: Lumafly build requires HKAPI-patched Assembly-CSharp.dll && \
		echo Please read CabbyCodes\lib-lumafly\README.txt for instructions. && \
		exit /b 1 \
	)
	@echo "Restoring NuGet packages for Lumafly..."
	dotnet restore $(SOLUTION_FILE) /p:Configuration=Lumafly
	@echo "Building for Lumafly (HKAPI)..."
	dotnet build $(CABBYCODES_PROJECT) --configuration Lumafly --no-restore
	dotnet build $(CABBYMENU_PROJECT) --configuration Lumafly --no-restore
	@echo "Lumafly build completed!"

# Build all versions (BepInEx 5, BepInEx 6, and Lumafly if available)
.PHONY: build-all-versions
build-all-versions: build-bepinex5 build-bepinex6
	@if exist "CabbyCodes\lib-lumafly\Assembly-CSharp.dll" (echo "Building Lumafly version..." && make build-lumafly) else (echo "NOTE: Skipping Lumafly build - HKAPI assembly not found in lib-lumafly")
	@echo "All available versions built successfully!"

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
# Usage: make deploy [VERSION=5|6] (defaults to 6)
.PHONY: deploy
deploy: build
	@echo Closing any existing Hollow Knight processes...
	@taskkill /f /im "hollow_knight.exe" >nul 2>&1 || exit /b 0
	@if "$(VERSION)" == "5" ( \
		echo Deploying $(PROJECT_NAME) for BepInEx 5 to Hollow Knight... \
	) else ( \
		echo Deploying $(PROJECT_NAME) for BepInEx 6 to Hollow Knight... \
	)
	@if not exist "$(BEPINEX_PLUGINS_PATH)" echo Error: BepInEx plugins directory "$(BEPINEX_PLUGINS_PATH)" not found. Please ensure Hollow Knight is installed and BepInEx is set up correctly. && exit /b 1
	@if exist "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" del /f /q "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll"
	@if exist "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" del /f /q "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll"
	@if "$(VERSION)" == "5" ( \
		copy /Y "CabbyCodes\bin\BepInEx5\$(TARGET_FRAMEWORK)\$(PROJECT_NAME).dll" "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" && \
		copy /Y "CabbyMenu\bin\BepInEx5\$(TARGET_FRAMEWORK)\CabbyMenu.dll" "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" \
	) else ( \
		copy /Y "CabbyCodes\bin\BepInEx6\$(TARGET_FRAMEWORK)\$(PROJECT_NAME).dll" "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" && \
		copy /Y "CabbyMenu\bin\BepInEx6\$(TARGET_FRAMEWORK)\CabbyMenu.dll" "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" \
	)
	@if exist "$(BEPINEX_PLUGINS_PATH)\$(PROJECT_NAME).dll" ( echo $(PROJECT_NAME).dll successfully copied to plugins folder. ) else ( echo Error: $(PROJECT_NAME).dll was not copied! )
	@if exist "$(BEPINEX_PLUGINS_PATH)\CabbyMenu.dll" ( echo CabbyMenu.dll successfully copied to plugins folder. ) else ( echo Error: CabbyMenu.dll was not copied! )
	@echo Deploy complete.

# Deploy Lumafly version to Hollow Knight Mods folder
# HKAPI mods are placed in their own subfolder within the Mods directory
.PHONY: deploy-lumafly
deploy-lumafly: build-lumafly
	@echo Closing any existing Hollow Knight processes...
	@taskkill /f /im "hollow_knight.exe" >nul 2>&1 || exit /b 0
	@echo Deploying $(PROJECT_NAME) for Lumafly to Hollow Knight...
	@if not exist "$(HOLLOW_KNIGHT_PATH)" echo Error: Hollow Knight directory "$(HOLLOW_KNIGHT_PATH)" not found. Please ensure Hollow Knight is installed. && exit /b 1
	@if not exist "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)" mkdir "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)"
	@if exist "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\$(PROJECT_NAME).dll" del /f /q "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\$(PROJECT_NAME).dll"
	@if exist "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\CabbyMenu.dll" del /f /q "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\CabbyMenu.dll"
	@copy /Y "CabbyCodes\bin\Lumafly\$(TARGET_FRAMEWORK)\$(PROJECT_NAME).dll" "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\$(PROJECT_NAME).dll"
	@copy /Y "CabbyMenu\bin\Lumafly\$(TARGET_FRAMEWORK)\CabbyMenu.dll" "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\CabbyMenu.dll"
	@copy /Y "$(HARMONY_DLL)" "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\0Harmony.dll"
	@if exist "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\$(PROJECT_NAME).dll" ( echo $(PROJECT_NAME).dll successfully copied to Mods\$(PROJECT_NAME) folder. ) else ( echo Error: $(PROJECT_NAME).dll was not copied! )
	@if exist "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\CabbyMenu.dll" ( echo CabbyMenu.dll successfully copied to Mods\$(PROJECT_NAME) folder. ) else ( echo Error: CabbyMenu.dll was not copied! )
	@if exist "$(LUMAFLY_MODS_PATH)\$(PROJECT_NAME)\0Harmony.dll" ( echo 0Harmony.dll successfully copied to Mods\$(PROJECT_NAME) folder. ) else ( echo Error: 0Harmony.dll was not copied! )
	@echo Lumafly deploy complete.

# Deploy Lumafly and run Hollow Knight
.PHONY: run-lumafly
run-lumafly: deploy-lumafly
	@echo Launching Hollow Knight via Steam...
	@start "" "steam://rungameid/367520"
	@echo Hollow Knight launched via Steam!

# Deploy and run Hollow Knight
# Usage: make run [VERSION=5|6] (defaults to 6)
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
	@echo "  all                    - Build the solution (default)"
	@echo "  build                  - Build the solution (defaults to BepInEx 6)"
	@echo "  build VERSION=5         - Build for BepInEx 5"
	@echo "  build VERSION=6         - Build for BepInEx 6"
	@echo "  debug                  - Build in Debug configuration"
	@echo "  build-cabbycodes       - Build only CabbyCodes project (BepInEx 6)"
	@echo "  build-cabbymenu        - Build only CabbyMenu project (BepInEx 6)"
	@echo "  build-bepinex5         - Build for BepInEx 5"
	@echo "  build-bepinex6         - Build for BepInEx 6"
	@echo "  build-lumafly          - Build for Lumafly (HKAPI)"
	@echo "  build-all-versions     - Build all versions (BepInEx 5, 6, and Lumafly)"
	@echo "  restore                - Restore NuGet packages"
	@echo "  clean                  - Clean build artifacts"
	@echo "  clean-all              - Deep clean (removes all build artifacts)"
	@echo "  test                   - Run tests"
	@echo "  publish                - Publish the project"
	@echo "  deploy                 - Deploy to Hollow Knight (defaults to BepInEx 6)"
	@echo "  deploy 5		        - Deploy BepInEx 5 version"
	@echo "  deploy 6		        - Deploy BepInEx 6 version"
	@echo "  deploy-lumafly         - Deploy Lumafly version to Mods folder"
	@echo "  run-lumafly            - Deploy Lumafly and run Hollow Knight"
	@echo "  run                    - Deploy and run Hollow Knight (defaults to BepInEx 6)"
	@echo "  run 5		            - Deploy and run BepInEx 5 version"
	@echo "  run 6		            - Deploy and run BepInEx 6 version"
	@echo "  check-sdk              - Check .NET SDK version"
	@echo "  info                   - Show project information"
	@echo "  rev x.x.x              - Update version numbers across projects"
	@echo "  package                - Package all versions (creates main ZIP + Lumafly ZIP)"
	@echo "  help                   - Show this help message"

# Update version numbers only
.PHONY: rev
rev:
	@if "$(filter-out $@,$(MAKECMDGOALS))" == "" (echo Error: Version parameter is required. Use: make rev x.x.x && exit /b 1)
	@echo "Updating version to $(filter-out $@,$(MAKECMDGOALS))..."
	@powershell -Command "(Get-Content 'CabbyCodes\CabbyCodes.csproj') -replace '<Version>.*</Version>', '<Version>$(filter-out $@,$(MAKECMDGOALS))</Version>' | Set-Content 'CabbyCodes\CabbyCodes.csproj'"
	@powershell -Command "(Get-Content 'CabbyMenu\CabbyMenu.csproj') -replace '<Version>.*</Version>', '<Version>$(filter-out $@,$(MAKECMDGOALS))</Version>' | Set-Content 'CabbyMenu\CabbyMenu.csproj'"
	@powershell -Command "(Get-Content 'CabbyCodes\Constants.cs') -replace 'public const string VERSION = \".*\";', 'public const string VERSION = \"$(filter-out $@,$(MAKECMDGOALS))\";' | Set-Content 'CabbyCodes\Constants.cs'"
	@echo "Version files updated successfully!"


# Catch-all target to handle version numbers (5, 6) as arguments
5 6:
	@:

# Catch-all target to prevent make from complaining about unknown targets when using positional args
%:
	@:

# Package the mod (create a single zip file with all versions)
# Will include Lumafly only if the HKAPI assembly is present
.PHONY: package
package: build-bepinex5 build-bepinex6
	@echo "Packaging $(PROJECT_NAME)..."
	@if not exist "Output\temp_package\CabbyCodes for BepInEx 5" mkdir "Output\temp_package\CabbyCodes for BepInEx 5"
	@if not exist "Output\temp_package\CabbyCodes for BepInEx 6" mkdir "Output\temp_package\CabbyCodes for BepInEx 6"
	@copy /Y "CabbyCodes\bin\BepInEx5\$(TARGET_FRAMEWORK)\$(PROJECT_NAME).dll" "Output\temp_package\CabbyCodes for BepInEx 5\$(PROJECT_NAME).dll"
	@copy /Y "CabbyMenu\bin\BepInEx5\$(TARGET_FRAMEWORK)\CabbyMenu.dll" "Output\temp_package\CabbyCodes for BepInEx 5\CabbyMenu.dll"
	@copy /Y "CabbyCodes\bin\BepInEx6\$(TARGET_FRAMEWORK)\$(PROJECT_NAME).dll" "Output\temp_package\CabbyCodes for BepInEx 6\$(PROJECT_NAME).dll"
	@copy /Y "CabbyMenu\bin\BepInEx6\$(TARGET_FRAMEWORK)\CabbyMenu.dll" "Output\temp_package\CabbyCodes for BepInEx 6\CabbyMenu.dll"
	@if exist "CabbyCodes\lib-lumafly\Assembly-CSharp.dll" (echo "Building and including Lumafly version..." && make build-lumafly && if not exist "Output\temp_package\CabbyCodes for Lumafly" mkdir "Output\temp_package\CabbyCodes for Lumafly" && copy /Y "CabbyCodes\bin\Lumafly\$(TARGET_FRAMEWORK)\$(PROJECT_NAME).dll" "Output\temp_package\CabbyCodes for Lumafly\$(PROJECT_NAME).dll" && copy /Y "CabbyMenu\bin\Lumafly\$(TARGET_FRAMEWORK)\CabbyMenu.dll" "Output\temp_package\CabbyCodes for Lumafly\CabbyMenu.dll" && copy /Y "$(HARMONY_DLL)" "Output\temp_package\CabbyCodes for Lumafly\0Harmony.dll") else (echo "NOTE: Skipping Lumafly - HKAPI assembly not found in lib-lumafly")
	@powershell -Command "$$modVer=([regex]::Match([IO.File]::ReadAllText('CabbyCodes/CabbyCodes.csproj'),'<Version>([^<]+)</Version>')).Groups[1].Value; $$zipPath='Output\$(PROJECT_NAME)_v'+$$modVer+'.zip'; $$lumaflyZip='Output\$(PROJECT_NAME)_Lumafly_v'+$$modVer+'.zip'; $$skipped=@(); $$folders=@('Output\temp_package\CabbyCodes for BepInEx 5','Output\temp_package\CabbyCodes for BepInEx 6'); if(Test-Path 'Output\temp_package\CabbyCodes for Lumafly'){$$folders+='Output\temp_package\CabbyCodes for Lumafly'}; if(Test-Path $$zipPath){Write-Host 'SKIPPED:' $$zipPath 'already exists'; $$skipped+=$$zipPath}else{Compress-Archive -Path $$folders -DestinationPath $$zipPath; Write-Host 'Package created:' $$zipPath}; if(Test-Path 'Output\temp_package\CabbyCodes for Lumafly'){if(Test-Path $$lumaflyZip){Write-Host 'SKIPPED:' $$lumaflyZip 'already exists'; $$skipped+=$$lumaflyZip}else{Compress-Archive -Path 'Output\temp_package\CabbyCodes for Lumafly\*' -DestinationPath $$lumaflyZip; Write-Host 'Lumafly package created:' $$lumaflyZip}}; if(Test-Path 'Output\temp_package'){Remove-Item -Recurse -Force 'Output\temp_package'}; if($$skipped.Count -gt 0){Write-Host ''; Write-Host 'ERROR: The following zip files already exist and were skipped:' -ForegroundColor Red; $$skipped | ForEach-Object {Write-Host '  -' $$_ -ForegroundColor Red}; Write-Host 'Delete these files or update the version number before packaging.' -ForegroundColor Red; exit 1}"

