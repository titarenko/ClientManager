###############################################################################
# Constants
###############################################################################

$defaultPackageName = "ClientManagerPackage"
$defaultOutputDir = "C:\$defaultPackageName"

###############################################################################
# Functions
###############################################################################

function readInput ($message, $defaultValue) {
	$value = Read-Host "$message (default is $defaultValue)"
	if (-not $value) {
		$value = $defaultValue
	}
	return $value
}

function anyKeyTo ($message) {
	"Press any key to $message..."
	$notUsed = $Host.UI.RawUI.ReadKey()
}

function copyOutput ($sourceDir, $outputDir) {
	& robocopy $sourceDir $outputDir *.asax *.dll *.config *.cshtml *.css *.js *.png *.jpg *.jpeg *.gif /S /XD obj /XF *Debug* *Release* packages.config
	& robocopy "$sourceDir\Deployment" $outputDir Install.ps1
}

function cleanupOutput ($outputDir) {
}

function createZip ($packageName, $sourceDir, $outputDir) {
	$outputDir = Join-Path $outputDir $packageName
	& 7z a -tzip -mx9 $outputDir $sourceDir
}

function deleteOutput ($outputDir) {
	Remove-Item -Recurse -Force $outputDir
}

###############################################################################
# Script itself
###############################################################################

$packageName = readInput "Package name" $defaultPackageName
$baseDir = readInput "Output path" $defaultOutputDir
$outputDir = Join-Path $baseDir $packageName

$startTime = Get-Date
"Started on $startTime"

"Copying output..."
copyOutput .. $outputDir

"Cleaning up..."
cleanupOutput $outputDir

"Creating archive..."
createZip $packageName $outputDir $baseDir

"Deleting temporary files..."
deleteOutput $outputDir

$finishTime = Get-Date
"Finished on $finishTime"
"Time taken: {0}" -f ($finishTime - $startTime)

anyKeyTo "exit"