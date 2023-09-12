# Wave Image Renderer

<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/tree.png" width="400" height="260"> <img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/wave.png" width="400" height="260">

A program converting regular images into an interesting art style consisting of many different lines of varying thickness and frequencies to convey color intensity.

The user can upload their image into the software where they can adjust the image generation settings before saving the newly generated image. 

The project also has the potential to render video / gameplay in this way although this may not be very speed effective (roughly 10fps ).


## Background 

I saw some interesting Graphic designs on one of my friends T-Shirts (Images below) upon closer inspection the design utilised many lines with varying frequencies / thicknesses to create an illusion that appears as an image.

### Example 1
This Design utilises many linear lines that vary in thickness to convey an image.
 
<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/IMG_1130.jpg" width="300" height="400"> <img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/IMG_1132.jpg" width="300" height="400">

### Example 2
This design however uses sinusoidal waves instead of straight lines in combination with thickness to convey the image.

<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/IMG_1131.jpg" width="330" height="400">

### My Approach
I decided to use my own approach that utilises both varying frequencies of sine waves (higher frequency means higher intensity lines) and varying thickness of lines as this provided what I thought was a more interesting and unique effect.

Given the lines are comprised of many overlayed varying radius spheres it is also possible to create a larger gap between all the circles and only rely on varying radii of circles to utilise a more pop art effect as seen here

<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/einstein%20dots.png" width="300" height="400"> <img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/skiing%20dots.png" width="600" height="400"> 

Although the monotone look is more striking it is also possible to colorise the effect. Given the lines already represent intensity the intensity of all colours should be the same high level. This can be achieved by using HSV colours where the 'value' component is set to 1.0 . As seen below


<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/skiier%20colour.png" width="700" height="400">

## Other Examples

<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/smile.png" width="400" height="400">

<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/einstein%20bw.png" width="300" height="400">

<img src="https://github.com/edf1101/Wave-Image-Renderer/blob/main/repoImages/color%20waves.png" width="300" height="400">


