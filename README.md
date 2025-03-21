# Index Customization Project

> [!IMPORTANT]
> This is the [`custom-models`](https://github.com/zaynethedev/Index_CustomizationProject/tree/custom-models) branch of the customization project. <br/>
> Everything here is unfinished and unstable, so if you would like to go back to the stable version, click [here](https://github.com/zaynethedev/Index_CustomizationProject). 

This is a project to allow users to create and bundle custom themes, platforms, and ~~models~~ for the Index panel. <br/>
This README is a documentation on how to make and bundle these custom objects for Index.

Current options: <br/>
[`Theme Creation`](#Theme-Creation) <br/>
[`Platform Creation`](#Platform-Creation) <br/>
[`Model Creation`](#Model-Creation) <br/>

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


Now whenever you're ready, copy this file into the `Index/Loaders/Themes` folder in your BepInEx plugins. (If this folder doesn't exist, create it, or check your Index version.) <br/>

![Files](https://github.com/user-attachments/assets/b1e72c84-83c1-4c46-a425-0dcf7d594305) <br/>


Now when you load into your game, you should be able to activate your theme.

<br/>
<br/>

# Platform Creation

> [!TIP]
> You can use custom platform models as long as the model (in your 3d modeling software) or the gameobject (in Unity) has a scale of [0.3, 0.06, 0.3] or less, and as long as you add the `Index Custom Platform` script to your object.

> [!WARNING]
> The loader for platforms divides your platform model by `[0.3, 0.06, 0.3]`, so make sure that your platform size is the exact same as the prefab size. (If the size of your model in your 3d modeling software matches 0.3, 0.06, 0.3, the ideal scale for your platform in the Unity editor should be 50, 50, 50)

To make a theme, drag and drop the `Custom Platform` prefab into your scene view. <br/>

![DragNDrop](https://github.com/user-attachments/assets/49b70725-695e-49f7-aa89-27ae3f2d0886) <br/>


Now you can edit the variables inside of the `Index Custom Platform` script. <br/>

![PlatformInfo](https://github.com/user-attachments/assets/bcd7bac9-0c98-4045-b2ef-ebc048739945) <br/>


After you are done adding info and setting up the materials, go to the top of your screen and select `Index Customization Tools > Index Platform Packing`. <br/>

![Tabs](https://github.com/user-attachments/assets/9404dcb1-04b7-4f03-9f14-c1141744a96b) <br/>


Now press the button that reads the name of your theme object. <br/>

![ClickPlatform](https://github.com/user-attachments/assets/38eeb8e5-a8fe-40da-b0ef-0081a75c2bb1) <br/>


Now press the `BUNDLE` button and wait for it to finish. <br/>

![ClickBundle](https://github.com/user-attachments/assets/4099b4e1-336a-47d7-8818-4f57968d8ff0) <br/>


To find your packaged theme, go to `Assets/IndexPlatformBundles`, and find your theme. The theme file should have the extension of `.indexplatform`. <br/>

![ClickPackaged](https://github.com/user-attachments/assets/60cbd9fc-c14e-45d0-90aa-93ee906b5c02) <br/>


Now whenever you're ready, copy this file into the `Index/Loaders/Platforms` folder in your BepInEx plugins. (If this folder doesn't exist, create it, or check your Index version.) <br/>

![Files](https://github.com/user-attachments/assets/1799ce67-80ad-4e65-af08-510dae512e09) <br/>


Now when you load into your game, you should be able to activate your platform.

<br/>
<br/>

# Model Creation

> [!WARNING]
> This is an unstable version of model creation. It is recommended to wait until it is fully released to use this part of the customization project. Thank you!
