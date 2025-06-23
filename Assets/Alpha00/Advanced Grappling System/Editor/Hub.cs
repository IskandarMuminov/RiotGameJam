using UnityEditor;
using UnityEngine;

public class Hub : EditorWindow
{
    string[] toolbarStrings = { "Info", "Grappling Gun Setup","Grabbable Object Setup", "Documentation", "Links" };
    int toolbarsel = -1;
    //Gun settings
    public AudioSource grapplingSound;
    public AudioSource ungrapplingSound;
    public AudioSource assistSound;
    [SerializeField] GameObject gunModel;
    [SerializeField] Transform grapplingTip;
    [SerializeField] Transform grapplingCam;
    [SerializeField] Transform Player;
    [SerializeField] Rigidbody rb;
    [SerializeField] int assistSize = 1;
    [SerializeField] GameObject assistModel;
    [SerializeField] KeyCode grappleKey = KeyCode.Mouse0;
    private GrapplingGun grapplingGun;
    private grapple grabbableScript;
    [SerializeField] int spring = 22;
    [SerializeField] int damper = 7;
    [SerializeField] int massScale = 5;
    [SerializeField] GameObject objModel;
    [SerializeField] Transform mainObj;
    [SerializeField] float force = 3;
    [SerializeField] float drawSpeed = 1;
    [SerializeField] LineRenderer lr;
    public string nameGun = "Grappling Gun";
    [MenuItem("Tools/Alpha00/Advanced Grappling System Hub")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<Hub>("AGS Hub");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        toolbarsel = GUILayout.Toolbar(toolbarsel, toolbarStrings);
        GUILayout.EndVertical();
        if (toolbarsel >= 0)
        {
            switch (toolbarStrings[toolbarsel])
            {
                //Info Tab
                case "Info":
                    GUILayout.Label("Advanced Grappling System V 1.4", EditorStyles.layerMaskField);
                    GUILayout.Label("Made By Alpha00", EditorStyles.toolbarTextField);
                    GUILayout.Label("V 1.4:", EditorStyles.whiteLargeLabel);
                    GUILayout.Label(" Fixed all the scripts,updated and simplified the code", EditorStyles.radioButton);
                    GUILayout.Label(" Updated to Unity 6", EditorStyles.radioButton);
                    GUILayout.Label(" Retextured and reassigned materials", EditorStyles.radioButton);
                    GUILayout.Label(" Sound quality improved on grappling sounds", EditorStyles.radioButton);
                    GUILayout.Label("New Features:", EditorStyles.whiteLargeLabel);
                    GUILayout.Label(" Editor Redesign and missing text fixed", EditorStyles.radioButton);
                    GUILayout.Label(" Player Movement Overhaul", EditorStyles.radioButton);
                    GUILayout.Label(" NEW Demo Scene and Custom Models", EditorStyles.radioButton);
                    GUILayout.Label(" NEW Examples and NEW Models for the classic grappling gun", EditorStyles.radioButton);
                    GUILayout.Label(" NEW Grappling System functionality to be able to grab objects and get them to  come towards you", EditorStyles.radioButton);
                    GUILayout.Label(" Grabbable Object ADDED:The grappling gun is able to move them towards you", EditorStyles.radioButton);
                    GUILayout.Label(" Fixed a lot of other bugs", EditorStyles.radioButton);
                    GUILayout.Label("Advanced Grappling System", EditorStyles.centeredGreyMiniLabel);
                    GUILayout.Label("Made by Alpha00", EditorStyles.centeredGreyMiniLabel);
                    break;
                //Grappling Gun Setup Tab
                case "Grappling Gun Setup":
                    GUILayout.Label("Grappling Gun Setup", EditorStyles.layerMaskField);
                    GUILayout.Label("Setup your own grappling gun", EditorStyles.whiteLargeLabel);
                    //Base Settings
                    GUILayout.Label("Base Settings", EditorStyles.helpBox);
                    name = EditorGUILayout.TextField("Grappling Gun Name", name);
                    gunModel = EditorGUILayout.ObjectField("Gun Model", gunModel, typeof(GameObject), true) as GameObject;
                    grapplingSound = EditorGUILayout.ObjectField("Grappling Sound", grapplingSound, typeof(AudioSource), true) as AudioSource;
                    ungrapplingSound = EditorGUILayout.ObjectField("Grappling Stop Sound", ungrapplingSound, typeof(AudioSource), true) as AudioSource;
                    grapplingTip = EditorGUILayout.ObjectField("Grappling Tip", grapplingTip, typeof(Transform), true) as Transform;
                    grapplingCam = EditorGUILayout.ObjectField("Grappling Camera", grapplingCam, typeof(Transform), true) as Transform;
                    Player = EditorGUILayout.ObjectField("Player(Rigidbody)", Player, typeof(Transform), true) as Transform;
                    //Aim Assist
                    GUILayout.Label("Aim Assist Settings", EditorStyles.helpBox);
                    assistSize = EditorGUILayout.IntField("Aim Assist Range", assistSize);
                    assistModel = EditorGUILayout.ObjectField("Aim Assist Model", assistModel, typeof(GameObject), true) as GameObject;
                    assistSound = EditorGUILayout.ObjectField("Aim Assist Sound", assistSound, typeof(AudioSource), true) as AudioSource;
                    //Grappling Settings
                    GUILayout.Label("Grappling Settings", EditorStyles.helpBox);
                    grappleKey = (KeyCode)EditorGUILayout.EnumPopup("Grappling Key", grappleKey);
                    spring = EditorGUILayout.IntField("Spring", spring);
                    damper = EditorGUILayout.IntField("Damper", damper);
                    massScale = EditorGUILayout.IntField("Mass Scale", massScale);

                    if (GUILayout.Button("Create Grappling Gun"))
                    {
                        //base settings
                        gunModel.name = nameGun;
                        gunModel.AddComponent<GrapplingGun>();
                        grapplingGun = gunModel.GetComponent<GrapplingGun>();
                       // grapplingGun.grappleSound = grapplingSound;
                        //grapplingGun.ungrappleSound = ungrapplingSound;
                        grapplingGun.gunTip = grapplingTip;
                        grapplingGun.cameraPlayer = grapplingCam;
                        grapplingGun.player = Player;
                        //aim assist
                        grapplingGun.aimAssistSize = assistSize;
                        grapplingGun.debugAssist = assistModel;
                        //grapplingGun.aimAssistSound = assistSound;
                        //grappling settings
                        grapplingGun.spring = spring;
                        grapplingGun.damper = damper;
                        grapplingGun.massScale = massScale;
                    }
                    GUILayout.Label("Advanced Grappling System", EditorStyles.centeredGreyMiniLabel);
                    GUILayout.Label("Made by Alpha00", EditorStyles.centeredGreyMiniLabel);
                    break;

                    //Grabbable object setup tab
                case "Grabbable Object Setup":
                    GUILayout.Label("Grappling Object Setup", EditorStyles.layerMaskField);
                    GUILayout.Label("Setup your own grabbable object", EditorStyles.whiteLargeLabel);
                    //Base Settings
                    GUILayout.Label("Base Settings", EditorStyles.helpBox);
                    name = EditorGUILayout.TextField("Grabbable Object Name", name);
                    objModel = EditorGUILayout.ObjectField("Object Model", objModel, typeof(GameObject), true) as GameObject;
                    grapplingTip = EditorGUILayout.ObjectField("Grappling Tip", grapplingTip, typeof(Transform), true) as Transform;
                    Player = EditorGUILayout.ObjectField("Player", Player, typeof(Transform), true) as Transform;
                    rb = EditorGUILayout.ObjectField("Rigidbody", rb, typeof(Rigidbody), true) as Rigidbody;
                    force = EditorGUILayout.FloatField("Attraction Force", force);
                    drawSpeed = EditorGUILayout.FloatField("Line Drawing Speed", drawSpeed);
                    mainObj = EditorGUILayout.ObjectField("Main Object", mainObj, typeof(Transform), true) as Transform;
                    if (GUILayout.Button("Create Grabbable Object"))
                    {
                        //base settings
                        gunModel.name = name;
                        objModel.AddComponent<grapple>();
                        objModel.AddComponent<LineRenderer>();
                        grabbableScript = objModel.GetComponent<grapple>();
                        grabbableScript.gunTip = grapplingTip;
                        grabbableScript.player = Player;
                        grabbableScript.rb = rb;
                        grabbableScript.rendererLine = objModel.GetComponent<LineRenderer>();
                        grabbableScript.mainObject = mainObj;
                        grabbableScript.lineDrawSpeed = drawSpeed;
                    }
                    GUILayout.Label("Advanced Grappling System", EditorStyles.centeredGreyMiniLabel);
                    GUILayout.Label("Made by Alpha00", EditorStyles.centeredGreyMiniLabel);
                    break;
                //Documentation Tab
                case "Documentation":
                    GUILayout.Label("AGS Documentation", EditorStyles.layerMaskField);
                    GUILayout.Label("ONLINE DOCUMENTATION:", EditorStyles.largeLabel);
                    GUILayout.Label("https://alpha-5.gitbook.io/advanced-grappling-system/", EditorStyles.linkLabel);
                    GUILayout.Label("For offline documentation check:Alpha00/Advanced Grappling System/Advanced Grappling System - Documentation.pdf", EditorStyles.whiteLargeLabel);
                    GUILayout.Label("Advanced Grappling System", EditorStyles.centeredGreyMiniLabel);
                    GUILayout.Label("Made by Alpha00", EditorStyles.centeredGreyMiniLabel);
                    break;
                //Links Tab
                case "Links":
                    GUILayout.Label("Links", EditorStyles.layerMaskField);
                    GUILayout.Label("Online Documentation: https://alpha-5.gitbook.io/advanced-grappling-system/", EditorStyles.whiteLargeLabel);
                    GUILayout.Label("Publisher's Page: https://assetstore.unity.com/publishers/53742", EditorStyles.whiteLargeLabel);
                    GUILayout.Label("Other Assets: ", EditorStyles.toolbarSearchField);
                    GUILayout.Label("Universal Shop System and Kit: https://assetstore.unity.com/packages/templates/systems/advanced-puzzle-system-aps-194350", EditorStyles.whiteLargeLabel);
                    GUILayout.Label("Advanced Puzzle System: https://assetstore.unity.com/packages/templates/systems/universal-shop-system-and-kit-195931", EditorStyles.whiteLargeLabel);
                    GUILayout.Label("Advanced Grappling System", EditorStyles.centeredGreyMiniLabel);
                    GUILayout.Label("Made by Alpha00", EditorStyles.centeredGreyMiniLabel);
                    break;
            }
        }
    }
}
