# Index Custom Theme Project

This is a project to allow users to create and bundle themes for the Index panel. <br/>
This README is a documentation on how to make and bundle themes for Index.

Current options: <br/>
[`Theme Creation`](#Theme-Creation) <br/>
Platforms <br/>
~~Custom Models~~ [Coming Soon] <br/>

# Theme Creation

To make a theme, drag and drop the `Custom Theme` prefab into your scene view. <br/>

![DragNDrop](https://github.com/user-attachments/assets/d597ba6d-9ae8-4a33-840b-442a1f605ce4) <br/>


Now you can edit the variables inside of the `Index Custom Theme` script. <br/>

![ThemeInfo](https://github.com/user-attachments/assets/2de37424-8a57-4e07-bfc8-96cb18d54b85) <br/>


After you are done adding info and setting up the materials, go to the top of your screen and select `Index Customization Tools > Index Theme Packing`. <br/>

![Tabs](https://github.com/user-attachments/assets/b812f7aa-a316-4d29-ad37-d32523c76785) <br/>


Now press the button that reads the name of your theme object. <br/>

![ClickTheme](https://github.com/user-attachments/assets/b7293409-f323-43ba-9f68-200102c7ac68) <br/>


Now press the `BUNDLE` button and wait for it to finish. <br/>

![ClickBundle](https://github.com/user-attachments/assets/2818b1e0-c24a-466d-bf37-ff52541f273b) <br/>


To find your packaged theme, go to `Assets/IndexThemeBundles`, and find your theme. The theme file should have the extension of `.indextheme`. <br/>

![ClickPackaged](https://github.com/user-attachments/assets/49100fd6-38af-480d-b5a6-b8036fd3d2f0) <br/>


Now whenever you're ready, copy this file into the `Index/Themes` folder in your BepInEx plugins. (If this folder doesn't exist, create it, or check your Index version.) <br/>

![Files](https://github.com/user-attachments/assets/b1e72c84-83c1-4c46-a425-0dcf7d594305) <br/>


Now when you load into your game, you should be able to activate your theme.
