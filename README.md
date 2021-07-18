# Car Configurator

Unity project to configure a car
The configuration bar is built in html which communicates with Unity in 2 directions.

[Demo](http://bertyhell.s3-website.eu-central-1.amazonaws.com/projects/car-configurator-shd)

![Car with configuration bar at the bottom](readme_assets/screenshot.png)

## Development

Clone the repo

### Unity part of the site:
Open the game folder in Unity

Open level 1:
![](readme_assets/unity-level.png)

Open build settings from the unity file menu
* Select the WebGL platform
* Click the "Switch Platform" button
* Click the "Build And Run" button
![](readme_assets/unity-build-target.png)
  
Select the folder: site\public\game
![](readme_assets/unity-build-location.png)

Wait for the build to finish.

### React part of the site:

navigate to /site
make sure you have [NodeJS](https://nodejs.org/) installed
open a command line, navigate to the site folder and install the project dependencies:
```
npm install
```

run the site using the start command:
```
npm run start
```

The site should not be available in the browser under:
```
http://localhost:3000
```

## Hosting

If you want to host the site, make sure it is running correctly with the steps in the Development section.

Then build the site using:
```
npm run build
```

Upload the content of the site/build folder to your ftp host.

If you want to run the site under a certain folder instead of the root, set a homepage property in package.json, rebuild and reupload the build folder.
eg:
```
{
  "name": "site",
  "version": "0.1.0",
  "private": true,
  "homepage": "http://bertyhell.s3-website.eu-central-1.amazonaws.com/projects/car-configurator-shd",
  "dependencies": {
    ...
```
