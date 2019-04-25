using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.RevitAddIns;

//using Autodesk.RevitAddIns;

namespace BinLibrary.RevitHelper
{
    public static class UICommandHelper
    {
        /// <summary>
        /// 覆盖命令
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="commandId"></param>
        /// <param name="customCommandHandle"></param>
        /// <returns></returns>
        public static bool OverWriteCommand(this UIApplication uiapp, RevitCommandId commandId, EventHandler<ExecutedEventArgs> customCommandHandle)
        {
            bool result = false;
            if (!commandId.CanHaveBinding)
            {
                throw new Exception("Command can not have binding.");
            }
            result = true;
            var commandBinding = uiapp.CreateAddInCommandBinding(commandId);

            commandBinding.Executed += customCommandHandle;

            return result;
        }

        /// <summary>
        /// 解除所有revit命
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RemoveAllRevitCommands(this UIApplication uiapp )
        {
            //var temcount = 0;
            var commandIdnames = Enum.GetNames(typeof(CommandIds));
            foreach (var commandIdname in commandIdnames)
            {
                //MessageBox.Show(commandIdname);
                try
                {
                    RevitCommandId rvtcommandid = RevitCommandId.LookupCommandId(commandIdname);
                    //uiapp.RemoveAddInCommandBinding(rvtcommandid);
                    uiapp.CreateAddInCommandBinding(rvtcommandid).Executed += (m, n) => { };
                    //temcount++;
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            //MessageBox.Show("执行完毕:" + temcount + "：次");
        }

        /// <summary>
        /// 测试未通过
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RestoreAllRevitCommands(this UIApplication uiapp)
        {
            var temcount = 0;
            var commandIdnames = Enum.GetNames(typeof(CommandIds));
            foreach (var commandIdname in commandIdnames)
            {
                //MessageBox.Show(commandIdname);
                try
                {
                    RevitCommandId rvtcommandid = RevitCommandId.LookupCommandId(commandIdname);
                    //uiapp.RemoveAddInCommandBinding(rvtcommandid);
                    uiapp.RemoveAddInCommandBinding(rvtcommandid);
                    //uiapp.CreateAddInCommandBinding(rvtcommandid).Executed += (m, n) => { };
                    temcount++;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    continue;
                }
            }
            //MessageBox.Show(temcount.ToString());

        }
         
        /// <summary>
        /// 解除所有addin命令
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RemoveAllAddinCommands(this UIApplication uiapp)
        {
             //uiapp.RemoveAddInCommandBinding();
        }



        /// <summary>
        /// 覆盖命令
        /// </summary>
        /// <param name="uiapp"></param>
        /// <param name="commandId"></param>
        /// <param name="customCommandHandle"></param>
        /// <returns></returns>
        public static bool OverWriteCommand(this UIControlledApplication uiapp, RevitCommandId commandId, EventHandler<ExecutedEventArgs> customCommandHandle)
        {
            bool result = false;
            if (!commandId.CanHaveBinding)
            {
                throw new Exception("Command can not have binding.");
            }
            result = true;
            var commandBinding = uiapp.CreateAddInCommandBinding(commandId);

            commandBinding.Executed += customCommandHandle;

            return result;
        }

        /// <summary>
        /// 解除所有revit命
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RemoveAllRevitCommands(this UIControlledApplication uiapp)
        {
            //var temcount = 0;
            var commandIdnames = Enum.GetNames(typeof(CommandIds));
            foreach (var commandIdname in commandIdnames)
            {
                //MessageBox.Show(commandIdname);
                try
                {
                    RevitCommandId rvtcommandid = RevitCommandId.LookupCommandId(commandIdname);
                    //uiapp.RemoveAddInCommandBinding(rvtcommandid);
                    uiapp.CreateAddInCommandBinding(rvtcommandid).Executed += (m, n) => { };
                    //temcount++;
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            //MessageBox.Show("执行完毕:" + temcount + "：次");
        }

        /// <summary>
        /// 测试未通过
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RestoreAllRevitCommands(this UIControlledApplication uiapp)
        {
            var temcount = 0;
            var commandIdnames = Enum.GetNames(typeof(CommandIds));
            foreach (var commandIdname in commandIdnames)
            {
                //MessageBox.Show(commandIdname);
                try
                {
                    RevitCommandId rvtcommandid = RevitCommandId.LookupCommandId(commandIdname);
                    //uiapp.RemoveAddInCommandBinding(rvtcommandid);
                    uiapp.RemoveAddInCommandBinding(rvtcommandid);
                    //uiapp.CreateAddInCommandBinding(rvtcommandid).Executed += (m, n) => { };
                    temcount++;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    continue;
                }
            }
            //MessageBox.Show(temcount.ToString());

        }


        /// <summary>
        /// 解除所有addin命令
        /// </summary>
        /// <param name="uiapp"></param>
        public static void RemoveAllAddinCommands(this UIControlledApplication uiapp)
        {
            //uiapp.RemoveAddInCommandBinding();
        }

        public  enum CommandIds
        {
             ID_BUTTON_SELECT
            , ID_TOGGLE_ALLOW_LINK_SELECTION
            , ID_TOGGLE_ALLOW_UNDERLAY_SELECTION
            , ID_TOGGLE_ALLOW_PINNED_SELECTION
            , ID_TOGGLE_ALLOW_FACE_SELECTION
            , ID_TOGGLE_ALLOW_DRAG_ON_SELECTION
//            ,Dialog_Revit_FamilyBar:Control_Revit_PropertyPanelTypePropBtn
            , ID_TOGGLE_PROPERTIES_PALETTE
            , ID_OBJECTS_FAMILYHOST
            , ID_FAMILY_TYPE
            , ID_OBJECTS_EXTRUSION
            , ID_OBJECTS_BLEND
            , ID_OBJECTS_REVOLUTION
            , ID_OBJECTS_SWEEP
            , ID_OBJECTS_SWEPTBLEND
            , ID_OBJECTS_EXTRUSION_CUT
            , ID_OBJECTS_BLEND_CUT
            , ID_OBJECTS_REVOLUTION_CUT
            , ID_OBJECTS_SWEEP_CUT
            , ID_OBJECTS_SWEPTBLEND_CUT
            , ID_OBJECTS_CURVE
            , ID_OBJECTS_FAMSYM
            , ID_OBJECTS_PLACE_GROUP
            , ID_EDIT_GROUP
            , ID_OBJECTS_MODELTEXT
            , ID_OBJECTS_OPENING
            , ID_OBJECTS_CONTROL
            , ID_RBS_ADD_ELECTRICAL_CONNECTOR
            , ID_RBS_ADD_DUCT_CONNECTOR
            , ID_RBS_ADD_PIPE_CONNECTOR
            , ID_RBS_ADD_CABLETRAY_CONNECTOR
            , ID_RBS_ADD_CONDUIT_CONNECTOR
            , ID_OBJECTS_REFERENCE_CURVE
            , ID_OBJECTS_CLINE
            , ID_SKETCH_PLANE_TOOL
            , ID_SKETCH_GRID_VIS
            , ID_WORKPLANE_VIEW
            , ID_RVTDOC_LINK
            , ID_IFC_LINK
            , ID_FILE_CADFORMAT_LINK
            , ID_MARKUPS_LOAD
            , ID_RM_CREATE_DECAL
            , ID_SETTINGS_DECAL_TYPES
            , ID_POINT_CLOUD
            , ID_LINKED_DWG
            , ID_FILE_IMPORT
            , ID_ABS_IMPORT_GBXML
            , ID_INSERT_VIEWS_FROM_FILE
            , ID_INSERT_2D_ELEMENTS_FROM_FILE
            , ID_OBJECTS_RASTER
            , ID_EDIT_RASTER_SYMBOLS
            , ID_IMPORT_FAM_TYPES
            , ID_FAMILY_LOAD
            , ID_LOAD_GROUP
            , ID_ANNOTATIONS_DIMENSION_ALIGNED
            , ID_ANNOTATIONS_DIMENSION_ANGULAR
            , ID_ANNOTATIONS_DIMENSION_RADIAL
            , ID_ANNOTATIONS_DIMENSION_DIAMETER
            , ID_ANNOTATIONS_DIMENSION_ARCLENGTH
            , ID_SETTINGS_DIMENSIONS_LINEAR
            , ID_SETTINGS_DIMENSIONS_ANGULAR
            , ID_SETTINGS_DIMENSIONS_RADIAL
            , ID_SETTINGS_DIMENSIONS_DIAMETER
            , ID_OBJECTS_VIEW_DIR_SPEC_CURVE
            , ID_OBJECTS_FAMDETAIL
            , ID_OBJECTS_PLACE_DETAIL_GROUP
            , ID_OBJECTS_FAM_ANN_INST
            , ID_MASKING_REGION
            , ID_OBJECTS_TEXT_NOTE
            , ID_CHECK_SPELLING
            , ID_FIND_REPLACE
            , ID_VIEW_CATEGORY_VISIBILITY
            , ID_THIN_LINES
            , ID_VIEW_DEFAULT_3DVIEW
            , ID_VIEW_NEW_3DVIEW
            , ID_VIEW_NEW_WALKTHROUGH
            , ID_VIEW_NEW_SECTION
            , ID_NEW_PLAN_REGION
            , ID_VIEW_NEW_PLAN
            , ID_VIEW_NEW_RCP
            , ID_VIEW_NEW_STRUCTURAL_PLAN
            , ID_VIEW_NEW_AREASCHEME
            , ID_BUTTON_BRACING_ELEVATION
            , ID_BUTTON_INTERIOR_ROOM_ELEVATION
            , ID_PRJBROWSER_COPY
            , ID_DUPLICATE_WITH_DETAILING
            , ID_CREATE_DEPENDENT_VIEW
            , ID_VIEW_NEW_LEGEND
            , ID_VIEW_NEW_KEYNOTE_LEGEND
            , ID_VIEW_NEW_SCHEDULE
            , ID_VIEW_NEW_GRAPH_SCHED_COLUMN
            , ID_VIEW_NEW_MATERIAL_TAKEOFF
            , ID_VIEW_NEW_DRAWING_LIST
            , ID_VIEW_NEW_NOTE_BLOCK
            , ID_NEW_VIEWLIST
            , ID_WINDOW_CLOSE_HIDDEN
            , ID_WINDOW_NEW
            , ID_WINDOW_CASCADE
            , ID_WINDOW_TILE_VERT
            , ID_SHOW_VIEWCUBE
            , ID_NAVIGATION_BAR
            , ID_VIEW_PROJECTEXPLORER
            , ID_RBS_SYSTEM_NAVIGATOR
            , ID_TOGGLE_FABPART_BROWSER
            , ID_VIEW_STATUS_BAR
            , ID_STATUSBAR_WORKSETS
            , ID_STATUSBAR_DESIGNOPTIONS
            , ID_STARTUP_PAGE
            , ID_BROWSER_ORGANIZATION
            , ID_KEYBOARD_SHORTCUT_DIALOG
            , ID_SETTINGS_MATERIALS
            , ID_SETTINGS_OBJECTSTYLES
            , ID_SETTINGS_SNAPPING
            , ID_SETTINGS_UNITS
            , ID_FILE_EXTERNAL_PARAMETERS
            , ID_TRANSFER_PROJECT_STANDARDS
            , ID_PURGE_UNUSED
            , ID_STRUCTURAL_SETTINGS
            , ID_RBS_ELECTRICAL_LOAD_CLASSIFICATION
            , ID_RBS_ELECTRICAL_DEMAND_FACTOR
            , ID_RBS_PANEL_SCHEDULE_MANAGE_TEMPLATES
            , ID_RBS_PANEL_SCHEDULE_EDIT_A_TEMPLATE
            , ID_SETTINGS_FILLPATTERNS
            , ID_SETTINGS_ANALYSIS_DISPLAY_STYLES
            , ID_SETTINGS_LINE_STYLES
            , ID_SETTINGS_PENS
            , ID_SETTINGS_PEN_FONTS
            , ID_VIEWTAGSTYLES_SECTIONTAGS
            , ID_SETTINGS_ANNOTATIONS_LEADERS
            , ID_SETTINGS_DIMENSIONS
            , ID_SETTINGS_KEYNOTING
            , ID_DETAIL_LEVEL
            , ID_SETTINGS_ASSEMBLY_CODE
            , ID_STARTING_VIEW
            , ID_IDS_OF_SELECTION
            , ID_SELECT_BY_ID
            , ID_REVIEW_WARNINGS
            , ID_TOOLS_MACROS
            , ID_TOOLS_MACROSECURITY
//            ,CustomCtrl_%CustomCtrl_%Add-Ins%BIM 360%Glue
//            ,CustomCtrl_%CustomCtrl_%Add-Ins%BIM 360%Clash{0}Pinpoint
//            ,CustomCtrl_%CustomCtrl_%Add-Ins%BIM 360%Equipment{0}
//            ,Properties
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%HelloWorld
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%Snoop Db..
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%Snoop Current Selection...
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%Snoop Active View...
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%Snoop Application...
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%Test Framework...
//            ,CustomCtrl_%CustomCtrl_%CustomCtrl_%Add-Ins%Revit Lookup%Options%Events...
            , ID_EDIT_PASTE
            , ID_EDIT_PASTE_ALIGNED_LEVEL_BY_NAME
            , ID_EDIT_PASTE_ALIGNED_VIEWS_BY_NAME
            , ID_EDIT_PASTE_ALIGNED
            , ID_EDIT_PASTE_ALIGNED_SAME_PLACE
            , ID_EDIT_PASTE_ALIGNED_PICK_LEVEL
            , ID_EDIT_CUT
            , ID_EDIT_COPY
            , ID_EDIT_MATCH_TYPE
            , ID_EDIT_PAINT
            , ID_EDIT_UNPAINT
            , ID_COPING
            , ID_UNCOPING
            , ID_CUT_HOST
            , ID_UNCUT_HOST
            , ID_JOIN_ELEMENTS_EDITOR
            , ID_UNJOIN_ELEMENTS_EDITOR
            , ID_SPLIT_FACE
            , ID_ALIGN
            , ID_EDIT_MOVE
            , ID_OFFSET
            , ID_EDIT_MOVE_COPY
            , ID_EDIT_MIRROR
            , ID_EDIT_ROTATE
            , ID_EDIT_MIRROR_LINE
            , ID_TRIM_EXTEND_CORNER
            , ID_SPLIT
            , ID_EDIT_CREATE_PATTERN
            , ID_TRIM_EXTEND_SINGLE
            , ID_SPLIT_WITH_GAP
            , ID_EDIT_SCALE
            , ID_TRIM_EXTEND_MULTIPLE
            , ID_UNLOCK_ELEMENTS
            , ID_LOCK_ELEMENTS
            , ID_BUTTON_DELETE
            , ID_MEASURE_LINE
            , ID_MEASURE_PICK_LINES
            , ID_EDIT_CREATE_SIMILAR
            , ID_LOAD_INTO_PROJECTS
            , ID_LOAD_INTO_PROJECTS_CLOSE
            , ID_OBJECTS_FILLED_REGION
            , ID_OBJECTS_TAG_NOTE
            , ID_VIEW_NEW_REVISION_SCHEDULE
            , ID_TRUSS_TOP_CHORD_CURVE
            , ID_TRUSS_WEB_CURVE
            , ID_TRUSS_BOTTOM_CHORD_CURVE
            , ID_OBJECTS_REBAR_CURVE
            , ID_MAJOR_SEGMENT
            , ID_REBAR_SHAPE_ALLOWABLE_BAR_TYPES
            , ID_TURN_ON_MULTI_PLANAR_SHAPE
            , ID_SHAPE_STATUS
            , ID_LOAD_INTO_PROJECTS_REBAR_SHAPE
            , ID_LOAD_INTO_PROJECTS_CLOSE_REBAR_SHAPE
            , ID_OBJECTS_MASS
//            ,Dialog_Essentials_GenhostCreate:Control_Essentials_DrawOnFaces
//            ,Dialog_Essentials_GenhostCreate:Control_Essentials_DrawOnWorkplane
            , ID_OBJECTS_LEVEL
            , ID_SETTINGS_SUNANDSHADOWSSETTINGS
            , ID_GEO_MANAGE_LOCATIONS
            , ID_SPLIT_FACE_IN_NEW_FAMILIES
            , ID_REPEAT_COMPONENT
            , ID_OBJECTS_WALL
            , ID_OBJECTS_STRUCTURAL_WALL
            , ID_WALL_PICK_FACES
            , ID_OBJECTS_CORNICE
            , ID_OBJECTS_REVEAL
            , ID_OBJECTS_DOOR
            , ID_OBJECTS_WINDOW
            , ID_INPLACE_COMPONENT
            , ID_OBJECTS_STRUCTURAL_COLUMN
            , ID_OBJECTS_COLUMN
            , ID_ROOF_FOOTPRINT
            , ID_ROOF_EXTRUSION
            , ID_ROOF_PICK_FACES
            , ID_CREATE_SOFFIT_TB
            , ID_CREATE_FASCIA_TB
            , ID_CREATE_GUTTER_TB
            , ID_OBJECTS_CEILING
            , ID_OBJECTS_FLOOR
            , ID_OBJECTS_SLAB
            , ID_FLOOR_PICK_FACES
            , ID_CREATE_SLAB_EDGE_TB
            , ID_CURTA_SYSTEM_PICK_FACES
            , ID_OBJECTS_CW_GRID
            , ID_OBJECTS_MULLION
            , ID_OBJECTS_RAILING
            , ID_OBJECTS_RAILING_ON_HOST
            , ID_OBJECTS_RAMP
            , ID_OBJECTS_STAIRS
            , ID_OBJECTS_STAIRS_LEGACY
            , ID_OBJECTS_PROJECT_CURVE
            , ID_OBJECTS_ROOM
            , ID_OBJECTS_AREA_SEPARATION
            , ID_OBJECTS_ROOM_TAG
            , ID_BUTTON_TAG_ALL
            , ID_OBJECTS_AREA
            , ID_OBJECTS_AREASCHEME_BOUNDARY
            , ID_OBJECTS_AREA_TAG
            , ID_SETTINGS_COLORFILLSCHEMES
            , ID_SETTING_AREACALCULATIONS
            , ID_CREATE_OPENING_BY_FACE_TB
            , ID_CREATE_SHAFT_OPENING_TB
            , ID_CREATE_WALL_OPENING_TB
            , ID_CREATE_VERTICAL_OPENING_TB
            , ID_CREATE_DORMER_OPENING_TB
            , ID_OBJECTS_GRID
            , ID_OBJECTS_BEAM
            , ID_OBJECTS_TRUSS
            , ID_OBJECTS_BRACE
            , ID_OBJECTS_JOIST_SYSTEM
            , ID_OBJECTS_ISOLATED_FOOTING
            , ID_OBJECTS_CONTINUOUS_FOOTING
            , ID_OBJECTS_FOOTING_SLAB
            , ID_REBAR_PLACE
            , ID_REBAR_SYSTEM_SKETCH
            , ID_PATH_REIN_SKETCH
            , ID_FABRIC_DISTR_SYSTEM_SKETCH
            , ID_FABRIC_SINGLE_PLACEMENT
            , ID_CONCRETE_COVER
            , ID_REBAR_COVER_SETTINGS
            , ID_REBAR_ABBREVIATION
            , ID_NUMBERING_REINF_PARTITIONS
            , ID_RBS_MECHANICAL_DUCT
            , ID_DUCT_1LINE_PATH
            , ID_RBS_MECHANICAL_FITTING
            , ID_RBS_MECHANICAL_ACCESSORY
            , ID_RBS_MECHANICAL_CONVERTTOFLEX
            , ID_RBS_MECHANICAL_FLEX
            , ID_RBS_MECHANICAL_DIFFUSER
            , ID_RBS_MECHANICAL_EQUIPMENT
            , ID_RBS_PIPE_PIPE
            , ID_PIPE_1LINE_PATH
            , ID_PARALLEL_PIPES
            , ID_RBS_PIPE_FITTING
            , ID_RBS_PIPE_ACCESSORY
            , ID_RBS_PIPE_FLEX
            , ID_RBS_PLUMBING_FIXTURE
            , ID_RBS_SPRINKLER
            , ID_RBS_ELECTRICAL_WIRE
            , ID_RBS_ELECTRICAL_WIRE_SPLINE
            , ID_RBS_ELECTRICAL_WIRE_CHAMFER
            , ID_RBS_CABLE_TRAY
            , ID_RBS_CONDUIT
            , ID_PARALLEL_CONDUIT
            , ID_RBS_CABLETRAY_FITTING
            , ID_RBS_CONDUIT_FITTING
            , ID_RBS_ELECTRICAL_EQUIPMENT
            , ID_RBS_ELECTRICAL_DEVICE
            , ID_RBS_ELECTRICAL_COMMUNICATION_DEVICE
            , ID_RBS_ELECTRICAL_DATA_DEVICE
            , ID_RBS_ELECTRICAL_FIREALARM_DEVICE
            , ID_RBS_ELECTRICAL_LIGHTING_DEVICE
            , ID_RBS_ELECTRICAL_NURSECALL_DEVICE
            , ID_RBS_ELECTRICAL_SECURITY_DEVICE
            , ID_RBS_ELECTRICAL_TELEPHONE_DEVICE
            , ID_RBS_LIGHTING_FIXTURE
            , ID_ANNOTATIONS_DIMENSION_LINEAR
            , ID_SPOT_ELEVATION
            , ID_SPOT_COORDINATE
            , ID_SPOT_SLOPE
            , ID_SETTINGS_SPOT_ELEVATION
            , ID_SETTINGS_SPOT_COORDINATE
            , ID_SETTINGS_SPOT_SLOPE
            , ID_OBJECTS_DETAIL_CURVES
            , ID_OBJECTS_REPEATING_DETAIL
            , ID_OBJECTS_LEGEND_COMPONENT
            , ID_OBJECTS_CLOUD
            , ID_OBJECTS_INSULATION
            , ID_BUTTON_TAG
            , ID_BEAM_ANNOTATION_PLACEMENT
            , ID_TAG_MULTI
            , ID_TAG_MATERIAL
            , ID_OBJECTS_MEP_SPACE_TAG
            , ID_NEW_REFERENCE_VIEWER
            , ID_STAIRS_TRISER_NUMBER
            , ID_MULTI_REFERENCE_ANNOTATION_ALIGNED
            , ID_MULTI_REFERENCE_ANNOTATION_LINEAR
            , ID_KEYNOTE_ELEMENT
            , ID_KEYNOTE_MATERIAL
            , ID_KEYNOTE_USER
            , ID_SETTINGS_TAGS
            , ID_RBS_MECHANICAL_COLOR_FILL
            , ID_RBS_PIPING_COLOR_FILL
            , ID_OBJECTS_ROOM_FILL
            , ID_OBJECTS_SPAN_DIR_SYM
            , ID_OBJECTS_BEAM_SYSTEM_TAG
            , ID_OBJECTS_STAIRS_PATH
            , ID_OBJECTS_REBAR_SYSTEM_SPAN_SYM
            , ID_OBJECTS_PATH_REIN_SPAN_SYM
            , ID_FABRIC_SYSTEM_PLACE_SPANSYMBOL
            , ID_OBJECT_BC
            , ID_OBJECT_LOADS
            , ID_STRUCTURAL_SETTINGS_LOAD_CASES
            , ID_STRUCTURAL_SETTINGS_LOAD_COMB
            , ID_AM_EDIT_MODE_START
            , ID_RESET_ANALYTICAL_MODEL
            , ID_COMMAND_CHECK_MEMBER_SUPPORTS
            , ID_COMMAND_ANALYTICAL_CONSISTENCY_CHECKS
            , ID_OBJECTS_MEP_SPACE
            , ID_OBJECTS_MEP_SPACE_SEPARATION
            , ID_RBS_HVAC_ZONE
            , ID_RBS_HVAC_LOADS
            , ID_RBS_VIEW_PANEL_SCHEDULE
            , ID_MEP_DUCT_PRESSURE_LOSS_REPORT
            , ID_MEP_PIPE_PRESSURE_LOSS_REPORT
            , ID_RBS_CHECK_DUCT_SYSTEMS
            , ID_RBS_CHECK_PIPE_SYSTEMS
            , ID_RBS_CHECK_CIRCUITS
            , ID_SHOWDISCONNECTS_BUTTON
            , ID_CEA_VIEW_ENERGY_DATA
            , ID_ENABLE_ENERGY_ANALYSIS
            , ID_CEA_LAUNCH_ANALYSIS
            , ID_CEA_LAUNCH_DASHBOARD
            , ID_SHOW_MASS_NO_OVERRIDE
            , ID_SHOW_MASS_STANDARD
            , ID_SHOW_MASS_SURFACES
            , ID_SHOW_MASS_ZONES
            , ID_INPLACE_MASS
            , ID_SITE_TOPO_SURFACE
            , ID_SITE_COMPONENT
            , ID_SITE_PARKING_COMPONENT
            , ID_SITE_BUILDINGPAD
            , ID_SPLIT_SURFACE
            , ID_MERGE_SURFACE
            , ID_SITE_AREA
            , ID_SITE_PROPERTYLINE
            , ID_SITE_GRADE
            , ID_SITE_LABEL_CONTOURS
            , ID_WORKSET_EDITING_REQUESTS
            , ID_SETTINGS_PARTITIONS
            , ID_GRAY_INACTIVE_WORKSET_GRAPHICS
            , ID_FILE_SAVE_TO_MASTER
            , ID_FILE_SAVE_TO_MASTER_SHORTCUT
            , ID_WORKSETS_RELOAD_LATEST
            , ID_RELINQUISH_ALL_MINE
            , ID_MANAGE_CONNECTION_TO_A_REVIT_SERVER_ACCELERATOR
            , ID_PARTITIONS_SHOW_HISTORY
            , ID_FILE_BACKUPS
            , ID_COPY_WATCH_CURRENT
            , ID_COPY_WATCH_LINK
            , ID_RECONCILE_CURRENT
            , ID_RECONCILE_LINK
            , ID_COPY_SETTINGS
            , ID_TOGGLE_HOST_BY_LINK_BROWSER
            , ID_INTERFERENCE_CHECK
            , ID_INTERFERENCE_REPORT
            , ID_APPLY_VIEW_TEMPLATE
            , ID_CREATE_VIEW_TEMPLATE
            , ID_SETTINGS_VIEWTEMPLATES
            , ID_SETTINGS_FILTERS
            , ID_HIDE_ELEMENTS_EDITOR
            , ID_UNHIDE_ELEMENTS_EDITOR
            , ID_EDIT_CUT_BOUNDARY
            , ID_PHOTO_RENDERING_VIEW
            , ID_PHOTO_RENDER_IN_CLOUD
            , ID_PHOTO_RENDER_GALLERY
            , ID_VIEW_NEW_CALLOUT
            , ID_VIEW_NEW_CALLOUT_SKETCH
            , ID_VIEW_NEW_DRAFTING
            , ID_VOLUME_OF_INTEREST
            , ID_VIEW_NEW_SHEET
            , ID_VIEW_PLACE_VIEW
            , ID_OBJECTS_PLACE_TITLEBLOCK
            , ID_SETTINGS_REVISIONS
            , ID_CREATE_GUIDE_GRID
            , ID_NEW_MATCHLINE
            , ID_VIEWPORT_ACTIVATE
            , ID_VIEWPORT_ACTIVATE_BASE
            , ID_SETTINGS_PROJECT_INFORMATION
            , ID_SETTINGS_PROJECT_PARAMETERS
            , ID_RBS_MECHANICAL_SETTINGS
            , ID_RBS_ELECTRICAL_SETTINGS
            , ID_MEP_FABRICATION_SETTINGS
            , ID_RBS_BUILDINGSPACE_TYPE_SETTINGS
            , ID_SETTINGS_RENDERING_APPEARANCE
            , ID_SETTINGS_HALFTONE
            , ID_VIEWTAGSTYLES_CALLOUTTAGS
            , ID_VIEWTAGSTYLES_ELEVATIONTAGS
            , ID_GEO_ACQUIRE_COORDINATES
            , ID_GEO_PUBLISH_COORDINATES
            , ID_SPECIFY_SHARED_COORDINATES
            , ID_REPORT_SHARED_COORDINATES
            , ID_RELOCATE_SHARED_COORDINATES
            , ID_ROTATE_SHARED_COORDINATES
            , ID_MIRROR_PROJECT
            , ID_ROTATE_PROJECT
            , ID_EDIT_DESIGNOPTIONS
            , ID_EDIT_ADDTODESIGNOPTIONSET
            , ID_EDIT_PICKS_OPTION
            , ID_SETTINGS_PHASES
            , ID_FILTERS_SELECTION_SAVE
            , ID_RETRIEVE_FILTERS
            , ID_EDIT_SELECTIONS
            , ID_SWITCH_JOIN_ORDER_EDITOR
            , ID_JOIN_ROOF
            , ID_EDIT_BEAM_JOINS
            , ID_EDIT_WALL_JOINS
            , ID_EDIT_DEMOLISH
            , ID_VIEW_HIDE_ELEMENTS
            , ID_VIEW_HIDE_CATEGORY
            , ID_VIEW_GO_FILTER
            , ID_DISPLACEMENT_CREATE
            , ID_VIEW_OVERRIDE_ELEMENTS
            , ID_VIEW_OVERRIDE_CATEGORY
            , ID_EDIT_LINEWORK
            , ID_VIEW_APPLY_SELECTION_BOX
            , ID_CREATE_ASSEMBLY
            , ID_CREATE_PARTS
            , ID_END_INPLACE_FAMILY
            , ID_QUIT_INPLACE_FAMILY
            , ID_END_INPLACE_MASS
            , ID_QUIT_INPLACE_MASS
            , ID_RBS_TOGGLE_END_REFERENCE
            , ID_RBS_FINISH_JUSTIFY_SLOPE_EDITMODE
            , ID_RBS_CANCEL_JUSTIFY_SLOPE_EDITMODE
            , ID_CANCEL_SKETCH
            , ID_FINISH_SKETCH
            , ID_BUTTON_STAIRS_LANDING_SKETCH
            , ID_BUTTON_STAIRS_PATH
            , ID_QUIT_SKETCH_PROFILE
            , ID_FINISH_SKETCH_PROFILE
            , ID_OBJECTS_AXIS
            , ID_RBS_SYSTEM_INSPECTOR_INSPECT
            , ID_RBS_SYSTEM_INSPECTOR_FINISH
            , ID_RBS_SYSTEM_INSPECTOR_CANCEL
            , ID_DISPLACEMENT_ADD_ELEMENT
            , ID_DISPLACEMENT_REMOVE_ELEMENT
            , ID_DISPLACEMENT_FINISH_EDIT
            , ID_DISPLACEMENT_CANCEL_EDIT
            , ID_EDIT_BLEND_BASE
            , ID_EDIT_VERTEX_CONNECTIONS
            , ID_RAILING_SET_HOST
            , ID_RAILING_EDIT_JOINS
            , ID_RAILING_EDIT_MODE_TOGGLE_PREVIEW
            , ID_RAILPATH_EDITMODE_QUIT
            , ID_RAILPATH_EDITMODE_FINISH
            , ID_EDIT_CONTINUOUS_RAIL_JOINS
            , ID_QUIT_SWEEP
            , ID_FINISH_SWEEP
            , ID_SKETCH_2D_PATH
            , ID_PICK_PATH
            , ID_SWEEP_PROFILE
//            , Dialog_Essentials_ProfileFamTypeDbar:Control_Essentials_EditSketch
//            , Dialog_Essentials_ProfileFamTypeDbar:Control_Essentials_LoadFamily
            , ID_QUIT_PICK_BOUNDARY
            , ID_FINISH_PICK_BOUNDARY
            , ID_PICK_DORMER_BOUNDARY
            , ID_ADJUST_ANALYTICAL_WALL
            , ID_INLUDE_EXCLUDE_OPENING_FROM_ANALYTICAL_SURFACE
            , ID_CREATE_ANALYTICAL_LINK
            , ID_AM_EDIT_MODE_FINISH
            , ID_AM_EDIT_MODE_CANCEL
            , ID_ADD_LIGHT_TO_GROUP
            , ID_REMOVE_LIGHT_FROM_GROUP
            , ID_FINISH_LIGHT_GROUP_EDIT
            , ID_CANCEL_LIGHT_GROUP_EDIT
            , ID_QUIT_SWEPT_BLEND
            , ID_FINISH_SWEPT_BLEND
            , ID_SKETCH_PROFILE_1
            , ID_SKETCH_PROFILE_2
            , ID_REBAR_SYSTEM_PICK_DIRSPAN_EDGE
            , ID_MERGED_PART_CANCEL_EDIT
            , ID_MERGED_PART_FINISH_EDIT
            , ID_MERGED_PART_ADD
            , ID_MERGED_PART_REMOVE
            , ID_EDIT_CONTINUOUS_RAIL_PATH
            , ID_RESET_CONTINUOUS_RAIL
            , ID_SKETCH_SHAFTOPENING_SYMBOL
            , ID_EDITSFILTER_ADD_TO_FILTER
            , ID_EDITSFILTER_REMOVE_FROM_FILTER
            , ID_FINISH_SFILTER_EDIT_MODE
            , ID_CANCEL_SFILTER_EDIT_MODE
            , ID_SKETCH_DEFINE_SLOPE
            , ID_RBS_PANELSCHEDULE_FORMAT_TEMPLATE
            , ID_PANELSCHEDULE_PARAM_REMOVE
            , ID_PANELSCHEDULE_PARAM_FORMATUNITS
            , ID_PANELSCHEDULE_PARAM_CALCULATE
            , ID_PANELSCHEDULE_PARAM_COMBINE
            , ID_RBS_PANELSCHEDULE_FREEZE_COLANDROW
            , ID_PANELSCHEDULE_COLUMN_INSERT_LEFT
            , ID_PANELSCHEDULE_COLUMN_INSERT_RIGHT
            , ID_PANELSCHEDULE_COLUMN_DELETE
            , ID_PANELSCHEDULE_COLUMN_RESIZE
            , ID_PANELSCHEDULE_ROW_INSERT_ABOVE
            , ID_PANELSCHEDULE_ROW_INSERT_BELOW
            , ID_PANELSCHEDULE_ROW_DELETE
            , ID_PANELSCHEDULE_ROW_RESIZE
            , ID_PANELSCHEDULE_CELL_MERGE_UNMERGE
            , ID_PANELSCHEDULE_CELL_INSERT_GRAPHIC
            , ID_PANELSCHEDULE_CELL_EDIT_BORDER
            , ID_PANELSCHEDULE_CELL_EDIT_SHADING
            , ID_PANELSCHEDULE_TEXT_EDIT_FONT
            , ID_PANELSCHEDULE_TEXT_HORIZONTAL_ALIGN_LEFT
            , ID_PANELSCHEDULE_TEXT_HORIZONTAL_ALIGN_CENTER
            , ID_PANELSCHEDULE_TEXT_HORIZONTAL_ALIGN_RIGHT
            , ID_PANELSCHEDULE_TEXT_VERTICAL_ALIGN_TOP
            , ID_PANELSCHEDULE_TEXT_VERTICAL_ALIGN_MIDDLE
            , ID_PANELSCHEDULE_TEXT_VERTICAL_ALIGN_BOTTOM
            , ID_RBS_PANELSCHEDULE_TEMPLATEVIEW_FINISH
            , ID_RBS_PANELSCHEDULE_TEMPLATEVIEW_CANCEL
            , ID_RBS_SYSTEM_GROUP_EDIT_ADD_TO_GROUP
            , ID_RBS_SYSTEM_GROUP_EDIT_REMOVE_FROM_GROUP
            , ID_RBS_SYSTEM_GROUP_EDIT_BASE_OBJECT_SELECT
            , ID_RBS_FINISH_SYSTEM_GROUP_EDIT_MODE
            , ID_RBS_CANCEL_SYSTEM_GROUP_EDIT_MODE
            , ID_STAIRSEDITMODE_QUIT
            , ID_STAIRSEDITMODE_FINISH
            , ID_STAIRS_RUN
            , ID_STAIRS_LANDING
            , ID_STAIRS_SUPPORT
            , ID_STAIRS_CONVERT_TO_CUSTOM
            , ID_EDIT_SKETCH
            , ID_FLIP_STAIRS_DIRECTION
            , ID_SET_RAILINGS_TYPE
            , ID_BTN_FILTER_SELECTION
            , ID_SELECT_ALL_PASTED
            , ID_FINALIZE_SELECTED_ELEMENTS
            , ID_FINISH_PASTE_EDIT_MODE
            , ID_QUIT_PASTE_EDIT_MODE
            , ID_QUIT_PICK_PATH
            , ID_FINISH_PICK_PATH
            , ID_PICK_EDGES
            , ID_PICK_DIRSPAN_EDGE
            , ID_RESET_JOIST_SYSTEM
            , ID_FLOOR_PICK_DISPAN_EDGE
            , ID_FINISH_PROOF
            , ID_SKETCH_DEFINE_ROOF_SLOPE
            , ID_SKETCH_ALIGN_EAVES
//            ,Dialog_Essentials_AdjustHeightOrOverhang:Control_Essentials_RadioHeight
//            ,Dialog_Essentials_AdjustHeightOrOverhang:Control_Essentials_RadioOverhang
            , ID_RBS_DRAG_SURFACE_CONNECTOR
            , ID_RBS_FINISH_SURFACE_CONNECTOR_EDIT_MODE
            , ID_RBS_CANCEL_SURFACE_CONNECTOR_EDIT_MODE
            , ID_EDIT_BLEND_TOP
            , ID_TURN_ON_MULTI_PLANAR_REBAR
            , ID_TRUSS_TOP_CHORD_REFERENCE
            , ID_TRUSS_BOTTOM_CHORD_REFERENCE
            , ID_RBS_LAYOUTPATH_EDIT
            , ID_RBS_LAYOUTPATH_SOLUTIONS
            , ID_RBS_LAYOUTPATH_ADD
            , ID_RBS_LAYOUTPATH_REMOVE
            , ID_RBS_LAYOUTPATH_BASE
            , ID_RBS_LAYOUTPATH_BASE_REMOVE
            , ID_RBS_LAYOUTPATH_BASE_MODIFY
            , ID_RBS_LAYOUTPATH_FINISH
            , ID_RBS_LAYOUTPATH_CANCEL
            , ID_STRUCT_CONNECTIVITY_EDIT_MODE_FINISH
            , ID_STRUCT_CONNECTIVITY_EDIT_MODE_CANCEL
            , ID_STRUCT_CONN_OFFSET_REF_ENABLED
            , ID_STRUCT_CONN_OFFSET_REF_TOP_LEFT
            , ID_STRUCT_CONN_OFFSET_REF_MIDDLE_LEFT
            , ID_STRUCT_CONN_OFFSET_REF_BOTTOM_LEFT
            , ID_STRUCT_CONN_OFFSET_REF_TOP_CENTER
            , ID_STRUCT_CONN_OFFSET_REF_MIDDLE_CENTER
            , ID_STRUCT_CONN_OFFSET_REF_BOTTOM_CENTER
            , ID_STRUCT_CONN_OFFSET_REF_TOP_RIGHT
            , ID_STRUCT_CONN_OFFSET_REF_MIDDLE_RIGHT
            , ID_STRUCT_CONN_OFFSET_REF_BOTTOM_RIGHT
            , ID_BUTTON_STAIRS_RUN_BOUNDARY
            , ID_BUTTON_STAIRS_RISER
            , ID_RBS_PICKALIGNMENT_LINE
            , ID_JUSTIFY_TOP_LEFT
            , ID_JUSTIFY_MIDDLE_LEFT
            , ID_JUSTIFY_BOTTOM_LEFT
            , ID_JUSTIFY_TOP_CENTER
            , ID_JUSTIFY_MIDDLE_CENTER
            , ID_JUSTIFY_BOTTOM_CENTER
            , ID_JUSTIFY_TOP_RIGHT
            , ID_JUSTIFY_MIDDLE_RIGHT
            , ID_JUSTIFY_BOTTOM_RIGHT
            , ID_RBS_SWITCH_SYSTEM_EDIT_SWITCH_SELECT
            , ID_CANCEL_DPART_EDIT
            , ID_FINISH_DPART_EDIT
            , ID_SKETCH_DPART_EDIT
            , ID_DIVIDED_VOLUME_INTRST_LIST
            , ID_ADD_ELEMENT_TO_DIVISION
            , ID_REMOVE_ELEMENT_FROM_DIVISION
            , ID_PLACE_REF_POINT
            , ID_RBS_ZONE_ADD_SPACE
            , ID_RBS_ZONE_REMOVE_SPACE
            , ID_RBS_FINISH_ZONE_EDIT_MODE
            , ID_RBS_CANCEL_ZONE_EDIT_MODE
            , ID_EDITGROUP_ADD_TO_GROUP
            , ID_EDITGROUP_REMOVE_FROM_GROUP
            , ID_EDITGROUP_ATTACH_TO_GROUP
            , ID_FINISH_GROUP_EDIT_MODE
            , ID_CANCEL_GROUP_EDIT_MODE
            , ID_CANCEL_SURFACE
            , ID_FINISH_SURFACE
            , ID_SURFACE_POINTS
            , ID_SITE_IMPORT
            , ID_SITE_IMPORT_POINTS
            , ID_SITE_SIMPLIFY_SURFACE
            , ID_EDITGROUP_ATTACH_ELEM_TO_GROUP
            , ID_EDITGROUP_DETACH_ELEM_FROM_GROUP
            , ID_FINISH_ATTACHED_GROUP_EDIT_MODE
            , ID_CANCEL_ATTACHED_GROUP_EDIT_MODE
            , ID_QUIT_SKETCH_PATH
            , ID_FINISH_SKETCH_PATH
            , ID_RBS_CIRCUIT_GROUP_EDIT_ADD_TO_GROUP
            , ID_RBS_CIRCUIT_GROUP_EDIT_REMOVE_FROM_GROUP
            , ID_RBS_CIRCUIT_GROUP_EDIT_PANEL_SELECT
            , ID_RUN
            , ID_BUTTON_STAIRS_BOUNDARY
            , ID_CPWTCH_SETTINGS
            , ID_CPWTCH_COPY
            , ID_CPWTCH_WATCH
            , ID_FIND_FIXTURES
            , ID_CPWTCH_FINISH
            , ID_CPWTCH_QUIT
            , ID_ADD_ELEMENT_TO_ASSEMBLY
            , ID_REMOVE_ELEMENT_FROM_ASSEMBLY
            , ID_FINISH_ASSEMBLY_EDIT
            , ID_CANCEL_ASSEMBLY_EDIT
            , ID_AUTO_CEILING
            , ID_IMAGE_RAYTRACE_STOP
            , ID_IMAGE_RAYTRACE_SAVE
            , ID_IMAGE_RAYTRACE_CANCEL
            , ID_CREATEFORMELEM
            , ID_CREATEVOIDFORM
            , ID_OBJECTS_VOLUME_DIVIDER_SKETCH
            , ID_OBJECTS_VOLUME_DIVIDER_OBJECTS_TO_DIVIDE
            , ID_DIVIDED_ELEMENT_INTRST_PICK
            , ID_DIVIDED_ELEMENT_INTRST_LIST
            , ID_DISABLE_ANALYTICAL_MODEL
            , ID_RESET_SELECTED_ANALYTICAL_MODEL
            , ID_RESET_ANALYTICAL_LINKS
            , ID_CLEAN_MANUALLY_ADJUSTED
            , ID_OBJECTS_VOLUME_EDIT_DIVISION
            , ID_MERGED_PART_EDIT
            , ID_OBJECTS_PARTS_RESET
            , ID_OBJECTS_DIVIDE
            , ID_OBJECTS_MERGE
            , ID_SELECT_HOST
            , ID_PICK_HOST_IND_TAG
            , ID_OBJECTS_DIVIDED_SURFACE
            , ID_DIVIDE_PATH
            , ID_EDIT_CONTINUOUS_RAIL
            , ID_RESET_RAILING
            , ID_EXCLUDE_PART
            , ID_RESTORE_PART
            , ID_REMOVE_AREA_REINFORCEMENT_SYSTEM
            , ID_REMOVE_PATH_REINFORCEMENT_SYSTEM
            , ID_REMOVE_FABRIC_REINFORCEMENT_SYSTEM
            , ID_MULTI_REFERENCE_ANNOTATION_SELECT_TAG
            , ID_MULTI_REFERENCE_ANNOTATION_SELECT_DIMENSION
            , ID_MEP_SELECT_DUCT_PRESSURE_LOSS_REPORT
            , ID_MEP_SELECT_PIPE_PRESSURE_LOSS_REPORT
            , ID_RBS_MECHANICAL_DIFFUSER_TO_DUCT
            , ID_CHANGE_BEAM_END_REFERENCE
            , ID_RBS_CONVERTTO_PLACEHOLDERS
            , ID_RBS_CONVERTTO_DESIGNPARTS
            , ID_STRUCT_JUST_EDITOR_CARDINAL
            , ID_STRUCT_JUST_EDITOR_OFFSET_Y
            , ID_STRUCT_JUST_EDITOR_OFFSET_Z
            , ID_OVERRIDE_FAB_PART_BENDS
            , ID_OVERRIDE_FAB_PART_TEES
            , ID_OVERRIDE_FAB_PART_CROSSES
            , ID_RESTORE_IMAGE_SIZE
            , ID_RESIZE_IMAGE_ROWS
            , ID_COMPONENTREPEATER_REMOVEREPEATER
            , ID_FABRICATION_PART_TAKEOFF_DIALOG
            , ID_FABRICATION_PART_PRE_ROTATE
            , ID_FABRICATION_PART_TOGGLE_CONNECTOR
            , ID_FABRICATION_PART_SMART_SNAPPING
            , ID_FABRICATION_PART_SHOW_TOOLTIP
            , ID_FABRICATION_HANGER_ATTACH
            , ID_FABRICATION_OPTIMIZE_LENGTH
            , ID_FABRICATION_PART_POST_ROTATE
            , ID_FABRICATION_PART_POST_TOGGLE_CONNECTOR
//            ,Dialog_BuildingSystems_PanelScheduleTemplateDlgbar:Control_BuildingSystems_PanelScheduleChangeTemplate
//            ,Dialog_BuildingSystems_RbsCreateHvacSystemGroup:Control_BuildingSystems_RbsCreateDuctSystem
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreatePowerCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateDataCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateTelephoneCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateSecurityCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateFireAlarmCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateNurseCallCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateControlsCircuit
//            ,Dialog_BuildingSystems_RbsCreateCircuitGroup:Control_BuildingSystems_RbsCreateCommunicationCircuit
//            ,Dialog_BuildingSystems_RbsCreatePipingSystemGroup:Control_BuildingSystems_RbsCreatePipingSystem
//            ,Dialog_BuildingSystems_RbsCreateSwitchSystem:Control_BuildingSystems_RbsCreateSwitchSystem
//            ,Dialog_BuildingSystems_RbsSystemInspectBar:Control_BuildingSystems_RbsSystemInspectorInspect
//            ,Dialog_BuildingSystems_RbsDuctSizingBar:Control_BuildingSystems_RbsBtnSizing
//            ,Dialog_BuildingSystems_RbsEditSystemGroup:Control_BuildingSystems_RbsEditSystemGroup
//            ,Dialog_BuildingSystems_RbsEditSystemGroup:Control_BuildingSystems_RbsEditSystemGroupSelectBaseObject
//            ,Dialog_BuildingSystems_RbsEditSystemGroup:Control_BuildingSystems_RbsDisconnectBaseObject
//            ,Dialog_BuildingSystems_RbsEditSystemGroup:Control_BuildingSystems_SplitHvacSystemGroup
//            ,Dialog_BuildingSystems_RbsEditCircuitGroup:Control_BuildingSystems_RbsEditCircuitGroup
//            ,Dialog_BuildingSystems_RbsEditCircuitGroup:Control_BuildingSystems_RbsEditCircuitGroupSelectPanel
//            ,Dialog_BuildingSystems_RbsEditCircuitGroup:Control_BuildingSystems_RbsDisconnectPanel
//            ,Dialog_BuildingSystems_EditPipingSystemGroup:Control_BuildingSystems_EditPipingSystemGroup
//            ,Dialog_BuildingSystems_EditPipingSystemGroup:Control_BuildingSystems_EditPipingSystemGroupSelectBaseObject
//            ,Dialog_BuildingSystems_EditPipingSystemGroup:Control_BuildingSystems_EditPipingSystemGroupDisconnectBaseObject
//            ,Dialog_BuildingSystems_EditPipingSystemGroup:Control_BuildingSystems_SplitPipingSystemGroup
//            ,Dialog_BuildingSystems_RbsEditSwitchSystem:Control_BuildingSystems_RbsEditSwitchSystem
//            ,Dialog_BuildingSystems_RbsEditSwitchSystem:Control_BuildingSystems_RbsEditSwitchSystemSelectSwitch
//            ,Dialog_BuildingSystems_RbsEditSwitchSystem:Control_BuildingSystems_RbsEditSwitchSystemDisconnectSwitch
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_RbsBtnJustification
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_RbsBtnAutoconnect
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_InheritElevation
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_InheritSize
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_MaintainSlope
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_3dSnapping
//            ,Dialog_BuildingSystems_RbsLayoutOptionsBar:Control_BuildingSystems_RbsBtnTagonplacement
            , ID_SLOPE_PIPE_OFF
            , ID_SLOPE_PIPE_UP
            , ID_SLOPE_PIPE_DOWN
            , ID_SLOPE_PIPE_SHOW_TOOLTIP
//            ,Dialog_BuildingSystems_RbsLayoutpathBar:Control_BuildingSystems_RbsLayoutpath
//            ,Dialog_BuildingSystems_RbsLayoutpathBar:Control_BuildingSystems_RbsLayoutpath1LinePath
//            ,Dialog_BuildingSystems_RbsConnectIntoBar:Control_BuildingSystems_RbsConnectInto
//            ,Dialog_BuildingSystems_RbsRoutepathEdit:Control_BuildingSystems_RbsroutepathEdit
//            ,Dialog_BuildingSystems_RbsPanelScheduleCreateupdateDlgbar:Control_BuildingSystems_RbsPanelScheduleCreate
//            ,Dialog_BuildingSystems_RbsPanelScheduleCreateupdateDlgbar:Control_BuildingSystems_ChooseTemplate
//            ,Dialog_BuildingSystems_RbsPanelScheduleCreateupdateDlgbar:Control_BuildingSystems_RbsPanelScheduleUpdate
//            ,Dialog_BuildingSystems_RbsRoutepathJustify:Control_BuildingSystems_RbsroutepathJustify
//            ,Dialog_BuildingSystems_RbsRoutepathSlope:Control_BuildingSystems_RbsroutepathSlope
//            ,Dialog_BuildingSystems_ChangeCurveType:Control_BuildingSystems_ChangeType
//            ,Dialog_BuildingSystems_ChangeCurveType:Control_BuildingSystems_ReapplyType
//            ,Dialog_BuildingSystems_ConvertPlaceHolder:Control_BuildingSystems_ConvertPlaceHolder
//            ,Dialog_BuildingSystems_AddCap:Control_BuildingSystems_AddCap
//            ,Dialog_BuildingSystems_3dSnapping:Control_BuildingSystems_MaintainSlope
//            ,Dialog_BuildingSystems_3dSnapping:Control_BuildingSystems_3dSnapping
//            ,Dialog_BuildingSystems_RbsRoutepathModify:Control_BuildingSystems_RbsBtnUsingPlaceholders
//            ,Dialog_BuildingSystems_RbsRoutepathModify:Control_BuildingSystems_RbsBtnUsing3DElements
//            ,Dialog_BuildingSystems_RbsRoutepathModify:Control_BuildingSystems_RbsFinish
//            ,Dialog_BuildingSystems_RbsRoutepathModify:Control_BuildingSystems_RbsRoutingSolutionQuit
//            ,Dialog_BuildingSystems_RbsWireConversionBar:Control_BuildingSystems_RbsWireConversionArc
//            ,Dialog_BuildingSystems_RbsWireConversionBar:Control_BuildingSystems_RbsWireConversionChamfered
//            ,Dialog_BuildingSystems_RbsZoneEditBar:Control_BuildingSystems_RbsZoneEdit
//            ,Dialog_CurtainGridFamily_CurtainGridDialog:Control_CurtainGridFamily_WholeGrid
//            ,Dialog_CurtainGridFamily_CurtainGridDialog:Control_CurtainGridFamily_OneSeg
//            ,Dialog_CurtainGridFamily_CurtainGridDialog:Control_CurtainGridFamily_ExcludeSegs
//            ,Dialog_CurtainGridFamily_CurtainGridDialog:Control_CurtainGridFamily_FinishGirdLine
//            ,Dialog_CurtainGridFamily_CurtasysByFaceDbar:Control_CurtainGridFamily_SelMultiCheck
//            ,Dialog_CurtainGridFamily_CurtasysByFaceDbar:Control_CurtainGridFamily_CancelMultiPickDbar
//            ,Dialog_CurtainGridFamily_CurtasysByFaceDbar:Control_CurtainGridFamily_FinishMultiPickDbar
//            ,Dialog_CurtainGridFamily_CurtasysByFaceEditDbar:Control_CurtainGridFamily_CancelMultiPickDbar
//            ,Dialog_CurtainGridFamily_CurtasysByFaceEditDbar:Control_CurtainGridFamily_FinishMultiPickDbar
//            ,Dialog_CurtainGridFamily_CurtaSystemEdit:Control_CurtainGridFamily_EditCurtaSystem
//            ,Dialog_CurtainGridFamily_CurtaSystemEdit:Control_CurtainGridFamily_RedoCurtaSystem
//            ,Dialog_CurtainGridFamily_MullionJoinsBar:Control_CurtainGridFamily_SetMillionJoinsConti
//            ,Dialog_CurtainGridFamily_MullionJoinsBar:Control_CurtainGridFamily_SetMillionJoinsButt
//            ,Dialog_CurtainGridFamily_MullionPlacement:Control_CurtainGridFamily_RadioMullionLine
//            ,Dialog_CurtainGridFamily_MullionPlacement:Control_CurtainGridFamily_RadioMullionSegment
//            ,Dialog_CurtainGridFamily_MullionPlacement:Control_CurtainGridFamily_RadioMullionGrid
//            ,Dialog_Detail_TagNoteDlgBar:Control_Detail_EditSubstitutionText
//            ,Dialog_Detail_TagNoteUiDlgBar:Control_Detail_AlignLeft
//            ,Dialog_Detail_TagNoteUiDlgBar:Control_Detail_AlignCenter
//            ,Dialog_Detail_TagNoteUiDlgBar:Control_Detail_AlignRight
//            ,Dialog_Detail_TagNoteUiDlgBar:Control_Detail_AlignTop
//            ,Dialog_Detail_TagNoteUiDlgBar:Control_Detail_AlignMiddle
//            ,Dialog_Detail_TagNoteUiDlgBar:Control_Detail_AlignBottom
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_TextnoteAddleader
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_TextnoteAddleaderR
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_TextnoteAddLeaderArc
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_TextnoteAddLeaderArcR
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_TextnoteRmvleader
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_LeaderTopLeft
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_LeaderMiddleLeft
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_LeaderBottomLeft
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_LeaderTopRight
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_LeaderMiddleRight
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_LeaderBottomRight
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_AlignLeft
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_AlignCenter
//            ,Dialog_Detail_TextNoteDlgBar:Control_Detail_AlignRight
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextParaFormatNone
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextParaFormatBullet
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextParaFormatNumber
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextParaFormatLowercase
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextParaFormatUppercase
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextBold
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextItalic
//            ,Dialog_Revit_TextRtfUi:Control_Revit_TextUnderline
//            ,Dialog_Detail_TextNoteUiDlgBarFamily:Control_Detail_AlignLeft
//            ,Dialog_Detail_TextNoteUiDlgBarFamily:Control_Detail_AlignCenter
//            ,Dialog_Detail_TextNoteUiDlgBarFamily:Control_Detail_AlignRight
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_TextLeaderNone
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_TextLeaderOneseg
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_TextLeaderTwoseg
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_TextLeaderArc
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_LeaderTopLeft
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_LeaderMiddleLeft
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_LeaderBottomLeft
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_LeaderTopRight
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_LeaderMiddleRight
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_LeaderBottomRight
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_AlignLeft
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_AlignCenter
//            ,Dialog_Detail_TextNoteUiDlgBarNew:Control_Detail_AlignRight
            , ID_EDITGROUP_GROUP_EDIT
            , ID_EDITGROUP_UNGROUP
            , ID_REPLACE_GROUP_WITH_LINK
//            ,Dialog_ElementGroup_EditGroupNew:Control_ElementGroup_EditGroupAttachDetail
//            ,Dialog_ElementGroup_EditGroupNew:Control_ElementGroup_EditGroupDetachDetail
            , ID_RESTORE_ALL_EXCLUDED
//            ,Dialog_Essentials_ColorFillDlgBar:Control_Essentials_EditColorScheme
//            ,Dialog_Essentials_CurveVisibilityDbar:Control_Essentials_Visibility
//            ,Dialog_Essentials_DatumOptionBar:Control_Essentials_DatumPropagateExtents
            , IDC_EDIT_WITNESS_REFS
//            ,Dialog_Essentials_EditBlend:Control_Essentials_EditSketch2
//            ,Dialog_Essentials_EditBlend:Control_Essentials_EditSketch
//            ,Dialog_Essentials_EditBlend:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_EditSweep:Control_Essentials_EditSweep
//            ,Dialog_Essentials_EditSweep:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_EditSweptBlend:Control_Essentials_EditSweptBlend
//            ,Dialog_Essentials_EditSweptBlend:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_EditTextDbar:Control_Essentials_EditText
//            ,Dialog_Essentials_EditTextVisDbar:Control_Essentials_EditText
//            ,Dialog_Essentials_EditTextVisDbar:Control_Essentials_Visibility
//            ,Dialog_Essentials_ElemVisibilityDbar:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_FilterSelection:Control_Essentials_BtnFilterSelection
            , ID_VIEW_UNHIDE_ELEMENTS
            , ID_VIEW_UNHIDE_BY_CATEGORY
            , ID_VIEW_FRAME_REVEALHIDDEN
//            ,Dialog_Essentials_GenhostCreate:Control_Essentials_PlaceByInsert
//            ,Dialog_Essentials_GenhostCreate:Control_Essentials_PlaceByXface
//            ,Dialog_Essentials_GenhostCreate:Control_Essentials_PlaceByWorkplane
//            ,Dialog_Essentials_ImportInstanceExplode:Control_Essentials_ImportDeleteLayers
//            ,Dialog_Essentials_ImportInstanceExplode:Control_Essentials_ImportPartialExplode
//            ,Dialog_Essentials_ImportInstanceExplode:Control_Essentials_ImportExplode
//            ,Dialog_Essentials_ImportInstanceExplode:Control_Essentials_ImportQuery
//            ,Dialog_Essentials_ImportInstanceExplode:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_LightSourceDefinitionDlgBar:Control_Essentials_LightSourceDefinition
//            ,Dialog_Essentials_ModifyCurveDBar:Control_Essentials_CurveByPointsDissolveButton
//            ,Dialog_Essentials_PasteMode:Control_Essentials_PasteEditMode
//            ,Dialog_Essentials_PasteMode:Control_Essentials_FinishPaste
//            ,Dialog_Essentials_PasteMode:Control_Essentials_QuitPaste
//            ,Dialog_Essentials_PlanRegionViewRange:Control_Essentials_EditViewRange
//            ,Dialog_Essentials_PointElementFlexGeomDbar:Control_Essentials_PointElementMakeAdaptive
//            ,Dialog_Essentials_RbsConnectorLinkConnectorBar:Control_Essentials_RbsConnectorBtnPrimary
//            ,Dialog_Essentials_RbsConnectorLinkConnectorBar:Control_Essentials_RbsConnectorBtnLinkConnector
//            ,Dialog_Essentials_RbsConnectorLinkConnectorBar:Control_Essentials_RbsConnectorBtnRemoveLink
//            ,Dialog_Essentials_RbsConnectorPlacementBar:Control_Essentials_PlaceByXface
//            ,Dialog_Essentials_RbsConnectorPlacementBar:Control_Essentials_PlaceByWorkplane
//            ,Dialog_Essentials_ReconcileRvtLink:Control_Essentials_ReconcileCurrentLink
//            ,Dialog_Essentials_RehostPoint:Control_Essentials_RehostPoint
//            ,Dialog_Essentials_RvtLinkInstanceStdDbar:Control_Essentials_BindLinkAsGroup
//            ,Dialog_Essentials_SectionBar:Control_Essentials_SplitSectionSegment
//            ,Dialog_Essentials_SketchEdit:Control_Essentials_EditSketch
//            ,Dialog_Essentials_SketchEditVisib:Control_Essentials_EditSketch
//            ,Dialog_Essentials_SketchEditVisib:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_SketchEditVisibDepth:Control_Essentials_EditSketch
//            ,Dialog_Essentials_SketchEditVisibDepth:Control_Essentials_SketchVisibility
//            ,Dialog_Essentials_SplineEdit:Control_Essentials_SplineAddControl
//            ,Dialog_Essentials_SplineEdit:Control_Essentials_SplineDeleteControl
//            ,Dialog_Essentials_TagOnPlacement:Control_Essentials_TagOnPlacement
//            ,Dialog_Essentials_VertexLinksDbar:Control_Essentials_ShiftCw
//            ,Dialog_Essentials_VertexLinksDbar:Control_Essentials_ShiftCcw
//            ,Dialog_Essentials_VertexLinksDbar:Control_Essentials_ResetVtxConnections
//            ,Dialog_Essentials_VertexLinksDbar:Control_Essentials_VtxCtrlOnLoop0
//            ,Dialog_Essentials_VertexLinksDbar:Control_Essentials_VtxCtrlOnLoop1
//            ,Dialog_Essentials_WalkthroughFinish:Control_Essentials_CancelEditor
//            ,Dialog_Family_EditInsertRehostDbar:Control_Family_EditInsertRehost
//            ,Dialog_Family_FamInstDbar:Control_Family_EditInplaceFam
//            ,Dialog_Family_FamInstDbarRvtLink:Control_Family_EditInplaceFamRvtLink
//            ,Dialog_Family_FamInstGridDbar:Control_Family_PickHost
//            ,Dialog_Family_FaminstLeaderDlgbar:Control_Family_FaminstAddleader
//            ,Dialog_Family_FaminstLeaderDlgbar:Control_Family_FaminstRmvleader
//            ,Dialog_Family_FamInstMultihostPickMain:Control_Family_PickMainHost
//            ,Dialog_Family_FamInstOffsetDbar:Control_Family_PickHost
//            ,Dialog_Family_FamInstPanelDbar:Control_Family_EditInplaceFam
//            ,Dialog_Family_ManyInstDbar:Control_Family_NewInplaceAny
//            ,Dialog_Family_NewIsoFooting:Control_Family_CreateFootingByGridIntersection
//            ,Dialog_Family_NewIsoFooting:Control_Family_CreateFootingFromStructuralColumn
//            ,Dialog_Family_NewStructuralColumn:Control_Family_GridIntersection
//            ,Dialog_Family_NewStructuralColumn:Control_Family_FromArchColumn
//            ,Dialog_Family_SelectCtrlIcon:Control_Family_RadioVerArrow
//            ,Dialog_Family_SelectCtrlIcon:Control_Family_RadioVerArrow2
//            ,Dialog_Family_SelectCtrlIcon:Control_Family_RadioHorArrow
//            ,Dialog_Family_SelectCtrlIcon:Control_Family_RadioHorArrow2
//            ,Dialog_HostObj_CorniceReveal:Control_HostObj_CorniceHorizontal
//            ,Dialog_HostObj_CorniceReveal:Control_HostObj_CorniceVertical
//            ,Dialog_HostObj_CorniceReveal:Control_HostObj_FinishCorniceReveal
//            ,Dialog_HostObj_SketchEditElevationRem:Control_HostObj_EditElevationSketch
//            ,Dialog_HostObj_SketchEditElevationRem:Control_HostObj_RemoveElevationSketch
//            ,Dialog_HostObj_CreateWallOpening:Control_HostObj_CreateWallOpening
//            ,Dialog_HostObj_CreateWallOpening:Control_HostObj_ExtendWalls
//            ,Dialog_HostObj_CreateWallOpening:Control_HostObj_UnextendWalls
//            ,Dialog_HostObj_DividedPathEditRefSelectionDbar:Control_HostObj_DividedPathFinishMultiPickDbar
//            ,Dialog_HostObj_DividedPathEditRefSelectionDbar:Control_HostObj_DividedPathCancelMultiPickDbar
//            ,Dialog_HostObj_DividedSurfaceDirection1Bar:Control_HostObj_GridlinesU
//            ,Dialog_HostObj_DividedSurfaceDirection1Bar:Control_HostObj_GridlinesV
//            ,Dialog_HostObj_DividedSurfaceDirection1Bar:Control_HostObj_IntrstPick
//            ,Dialog_HostObj_DividedSurfaceDirection1Bar:Control_HostObj_IntrstList
//            ,Dialog_HostObj_DividedSurfaceDisplayPropertiesBar:Control_HostObj_ShowSurf
//            ,Dialog_HostObj_DividedSurfaceDisplayPropertiesBar:Control_HostObj_ShowPtrn
//            ,Dialog_HostObj_DividedSurfaceDisplayPropertiesBar:Control_HostObj_ShowComp
//            ,Dialog_HostObj_DividedSurfaceEditRefSelectionDbar:Control_HostObj_DividedSurfaceFinishMultiPickDbar
//            ,Dialog_HostObj_DividedSurfaceEditRefSelectionDbar:Control_HostObj_DividedSurfaceCancelMultiPickDbar
//            ,Dialog_HostObj_EditCornice:Control_HostObj_EditWallSweep
//            ,Dialog_HostObj_EditCornice:Control_HostObj_ChangeSweepReturns
//            ,Dialog_HostObj_EditCurtainGridDialog:Control_HostObj_CurtaGridLineAddRemoveSegs
//            ,Dialog_HostObj_EditDormerOpening:Control_HostObj_EditDormerOpening
//            ,Dialog_HostObj_EditMiterSweepHostDialog:Control_HostObj_EditSweepHost
//            ,Dialog_HostObj_EditMiterSweepHostDialog:Control_HostObj_MiterSweepHost
//            ,Dialog_HostObj_EditReveal:Control_HostObj_EditWallSweep
//            ,Dialog_HostObj_EditReveal:Control_HostObj_ChangeSweepReturns
//            ,Dialog_HostObj_EditSweepHostDialog:Control_HostObj_EditSweepHost
//            ,Dialog_HostObj_EditWallsweepReturns:Control_HostObj_ChangeSweepReturns
//            ,Dialog_HostObj_FaceRoofEdit:Control_HostObj_EditFaceRoof
//            ,Dialog_HostObj_FaceRoofEdit:Control_HostObj_RedoFaceRoof
//            ,Dialog_HostObj_FloorByFaceDbar:Control_HostObj_SelMultiCheck
//            ,Dialog_HostObj_FloorByFaceDbar:Control_HostObj_CancelMultiPickDbar
//            ,Dialog_HostObj_FloorByFaceDbar:Control_HostObj_FinishMultiPickDbar
//            ,Dialog_HostObj_MiterSweepHostDialog:Control_HostObj_MiterVertical
//            ,Dialog_HostObj_MiterSweepHostDialog:Control_HostObj_MiterHorizontal
//            ,Dialog_HostObj_MiterSweepHostDialog:Control_HostObj_MiterNormalToPath
//            ,Dialog_HostObj_OpeningDbar:Control_HostObj_EditSketch
//            ,Dialog_HostObj_OpeningWRehostDbar:Control_HostObj_EditSketch1
//            ,Dialog_HostObj_OpeningWRehostDbar:Control_HostObj_PickHost
//            ,Dialog_HostObj_RedoFaceBased:Control_HostObj_RedoFaceBased
//            ,Dialog_HostObj_RoofByFaceDbar:Control_HostObj_SelMultiCheck
//            ,Dialog_HostObj_RoofByFaceDbar:Control_HostObj_CancelMultiPickDbar
//            ,Dialog_HostObj_RoofByFaceDbar:Control_HostObj_FinishMultiPickDbar
//            ,Dialog_HostObj_RoofByFaceEditDbar:Control_HostObj_CancelMultiPickDbar
//            ,Dialog_HostObj_RoofByFaceEditDbar:Control_HostObj_FinishMultiPickDbar
//            ,Dialog_HostObj_SketchEditElevation:Control_HostObj_EditElevationSketch
//            ,Dialog_HostObj_SketchEditElevation:Control_HostObj_ExtendWalls
//            ,Dialog_HostObj_SketchEditElevation:Control_HostObj_UnextendWalls
//            ,Dialog_HostObj_SketchEditElevationRem:Control_HostObj_ExtendWalls
//            ,Dialog_HostObj_SketchEditElevationRem:Control_HostObj_UnextendWalls
//            ,Dialog_HostObj_SketchEditPlanNoStuff:Control_HostObj_EditSketch1
//            ,Dialog_HostObj_SketchEditPlanNoStuff:Control_HostObj_EditSketch2
//            ,Dialog_HostObj_SlabShapeMainEditorBar:Control_HostObj_ModifySlabShapeSubs
//            ,Dialog_HostObj_SlabShapeMainEditorBar:Control_HostObj_DrawSlabShapeVertex
//            ,Dialog_HostObj_SlabShapeMainEditorBar:Control_HostObj_DrawSlabShapeLines
//            ,Dialog_HostObj_SlabShapeMainEditorBar:Control_HostObj_PickSlabShapeSupports
//            ,Dialog_HostObj_SlabShapeMainEditorBar:Control_HostObj_RemoveSlabEdit
//            ,Dialog_HostObj_SweepHostDialog:Control_HostObj_FinishSweepHost
//            ,Dialog_Massing_RelatedHosts:Control_Massing_RelatedHosts
//            ,Dialog_Massing_CreateFaf:Control_Massing_CreateFaf
//            ,Dialog_Rebar_EditRebarSelectionDialogBar:Control_Rebar_EditRebarSelectShowAll
//            ,Dialog_Rebar_EditRebarSelectionDialogBar:Control_Rebar_EditRebarSelectHideAll
//            ,Dialog_Rebar_EditRebarSelectionDialogBar:Control_Rebar_EditRebarSelectFinish
//            ,Dialog_Rebar_EditRebarSelectionDialogBar:Control_Rebar_EditRebarSelectCancel
//            ,Dialog_Rebar_RebarBeamColRst6Dbar:Control_Rebar_RebarPlacePerpendicular
//            ,Dialog_Rebar_RebarBeamColRst6Dbar:Control_Rebar_RebarPlaceParallel
//            ,Dialog_Rebar_RebarFloorWallRst6Dbar:Control_Rebar_RebarPlacePerpendicular
//            ,Dialog_Rebar_RebarFloorWallRst6Dbar:Control_Rebar_RebarPlaceParallel
//            ,Dialog_Rebar_RebarFloorWallRst6Dbar:Control_Rebar_RebarSystemSketchHost
//            ,Dialog_Rebar_RebarFloorWallRst6Dbar:Control_Rebar_PathReinSketchHost
            , ID_FABRIC_DISTR_SYSTEM_SKETCH_HOST
//            ,Dialog_Rebar_RebarPlaceMultiPlanarOrientDbar:Control_Rebar_RebarPlaceLookingAtTop
//            ,Dialog_Rebar_RebarPlaceMultiPlanarOrientDbar:Control_Rebar_RebarPlaceLookingAtBottom
//            ,Dialog_Rebar_RebarPlaceMultiPlanarOrientDbar:Control_Rebar_RebarPlaceLookingAtFront
//            ,Dialog_Rebar_RebarPlaceMultiPlanarOrientDbar:Control_Rebar_RebarPlaceLookingAtBack
//            ,Dialog_Rebar_RebarPlaceMultiPlanarOrientDbar:Control_Rebar_RebarPlaceLookingAtRight
//            ,Dialog_Rebar_RebarPlaceMultiPlanarOrientDbar:Control_Rebar_RebarPlaceLookingAtLeft
//            ,Dialog_Rebar_RebarPlaceclicksOrSketchDbar:Control_Rebar_RebarSketch
//            ,Dialog_Rebar_RebarPlaceParaPerpDbar:Control_Rebar_RebarPlaceParallel1
//            ,Dialog_Rebar_RebarPlaceParaPerpDbar:Control_Rebar_RebarPlacePerpParallelToCover
//            ,Dialog_Rebar_RebarPlaceParaPerpDbar:Control_Rebar_RebarPlacePerpendicular1
//            ,Dialog_Rebar_RebarPlacementPlaneDbar:Control_Rebar_RebarOnCurrentWorkPlane
//            ,Dialog_Rebar_RebarPlacementPlaneDbar:Control_Rebar_RebarOnNearCover
//            ,Dialog_Rebar_RebarPlacementPlaneDbar:Control_Rebar_RebarOnFarCover
//            ,Dialog_Rebar_RebarRehostDialogBar:Control_Rebar_RebarRehost
            , ID_EDIT_REBAR_JOINS
            , ID_REBAR_PRESENTATION_ALL
            , ID_REBAR_PRESENTATION_FIRSTLAST
            , ID_REBAR_PRESENTATION_MIDDLE
            , ID_REBAR_PRESENTATION_SELECT
            , ID_RESET_TARGET
//            ,Dialog_Revit_CropRegionModeBar:Control_Revit_CropRegionBarSketch
//            ,Dialog_Revit_CropRegionModeBar:Control_Revit_CropRegionBarSketchRemove
//            ,Dialog_Revit_CropRegionBar:Control_Revit_CropRegionBarModify
//            ,Dialog_Revit_DrawOrder:Control_Revit_BringToFront
//            ,Dialog_Revit_DrawOrder:Control_Revit_BringForward
//            ,Dialog_Revit_DrawOrder:Control_Revit_SendToBack
//            ,Dialog_Revit_DrawOrder:Control_Revit_SendBackward
            , ID_MODIFY_ANALYSIS_DISPLAY_STYLE
//            ,Dialog_Revit_EditLoadedFamilyDbar:Control_Revit_EditLoadedFam
//            ,Dialog_Revit_EditSketchplaneDbar:Control_Revit_EditSketchplane
//            ,Dialog_Revit_EditWpRehostDbar:Control_Revit_EditWpRehost
//            ,Dialog_Revit_ExternalSheetFilter:Control_Revit_RoomScheduleFilterShow
//            ,Dialog_Revit_ExternalSheetFilter:Control_Revit_RoomScheduleFilterHide
//            ,Dialog_Revit_ExternalSheetFilter:Control_Revit_RoomScheduleFilterIsolate
//            ,Dialog_Revit_RemoiveWatchesDbar:Control_Revit_RemoveWatch
//            ,Dialog_Revit_RoomScheduleFilter:Control_Revit_RoomScheduleFilterShow
//            ,Dialog_Revit_RoomScheduleFilter:Control_Revit_RoomScheduleFilterHide
//            ,Dialog_Revit_RoomScheduleFilter:Control_Revit_RoomScheduleFilterIsolate
//            ,Dialog_Revit_ScheduleBar:Control_Revit_GroupHeaders
//            ,Dialog_Revit_ScheduleBar:Control_Revit_UngroupHeaders
//            ,Dialog_Revit_ScheduleBar:Control_Revit_InsertRow
//            ,Dialog_Revit_ScheduleBar:Control_Revit_DeleteRows
            , ID_HIDE_COLUMNS
            , ID_UNHIDE_ALL_COLUMNS
//            ,Dialog_Revit_ScheduleBar:Control_Revit_BtnScheduleShow
            , ID_SCHEDULE_CELL_EDIT_SHADING
            , ID_SCHEDULE_CELL_EDIT_BORDER
            , ID_SCHEDULE_RESET_OVERRIDE
            , ID_SCHEDULE_TEXT_EDIT_FONT
            , ID_SCHEDULE_TEXT_HORIZONTAL_ALIGN_LEFT
            , ID_SCHEDULE_TEXT_HORIZONTAL_ALIGN_CENTER
            , ID_SCHEDULE_TEXT_HORIZONTAL_ALIGN_RIGHT
            , ID_SCHEDULE_TEXT_VERTICAL_ALIGN_TOP
            , ID_SCHEDULE_TEXT_VERTICAL_ALIGN_MIDDLE
            , ID_SCHEDULE_TEXT_VERTICAL_ALIGN_BOTTOM
            , ID_SCHEDULE_PARAM_FORMATUNITS
            , ID_SCHEDULE_PARAM_CALCULATE
            , ID_SCHEDULE_CELL_MERGE_UNMERGE
            , ID_SCHEDULE_CELL_INSERT_GRAPHIC
            , ID_SCHEDULE_CLEAR_CELL
            , ID_SCHEDULE_COLUMN_INSERT_RIGHT
            , ID_SCHEDULE_COLUMN_DELETE
            , ID_SCHEDULE_COLUMN_RESIZE
            , ID_SCHEDULE_FREEZE_COLANDROW
            , ID_SCHEDULE_ROW_INSERT_ABOVE
            , ID_SCHEDULE_ROW_INSERT_BELOW
            , ID_INSERT_ROW
            , ID_DELETE_ROWS
            , ID_SCHEDULE_ROW_RESIZE
//            ,Dialog_Revit_ScheduleExplainErrorBar:Control_Revit_ScheduleExplainError
            , ID_SHOW_RELATED_WARNINGS
//            ,Dialog_Revit_WalkthroughEdit:Control_Revit_WalkthroughEdit
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnPrevCkp
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnPrevFrame
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnNextFrame
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnNextCkp
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnPlay
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnEditActivateview
//            ,Dialog_Revit_WalkthroughModify:Control_Revit_BtnResetDirections
//            ,Dialog_RoomAreaPlan_AreaTagDlgBar:Control_RoomAreaPlan_TagRoomOnPlacement
//            ,Dialog_RoomAreaPlan_RoomTagDlgBar:Control_RoomAreaPlan_FindAllRooms
//            ,Dialog_RoomAreaPlan_RoomTagDlgBar:Control_RoomAreaPlan_ShowAllRoomBounds
//            ,Dialog_RoomAreaPlan_RoomTagDlgBar:Control_RoomAreaPlan_TagRoomOnPlacement
            , ID_PICK_HOST_ROOM_TAG
//            ,Dialog_RoomAreaPlan_RoomTagDlgBar:Control_RoomAreaPlan_FindAllSpaces
//            ,Dialog_RoomAreaPlan_SketchCeiling:Control_RoomAreaPlan_SketchCeiling
//            ,Dialog_Sculpting_FormElemAddDeleteSplineControlDlgbar:Control_Sculpting_FormElemAddSplineControlButton
//            ,Dialog_Sculpting_FormElemAddDeleteSplineControlDlgbar:Control_Sculpting_FormElemDeleteSplineControlButton
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElementSketchEdit
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElementXRay
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElemSplitButton
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElemCleaveButton
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElemDissolveButton
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElemRehost
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElementConstrainProfile
//            ,Dialog_Sculpting_FormElementEditDlgbar:Control_Sculpting_FormElementUnconstrainProfile
//            ,Dialog_Site_PropertyLineEditDialog:Control_Site_EditSketch
//            ,Dialog_Site_PropertyLineEditDialog:Control_Site_PropertyLineEditTable
//            ,Dialog_Site_SiteRegionSketchEdit:Control_Site_EditSketch
//            ,Dialog_Site_SurfaceEdit:Control_Site_EditSurface
//            ,Dialog_Structural_AutoBeamSystemCreateDbar:Control_Structural_SketchJoistSystemFromAuto
//            ,Dialog_Structural_BeamSystemOptionDlg:Control_Structural_ExplodeBeamSystems
//            ,Dialog_Structural_ManyBcDbar:Control_Structural_PointBc
//            ,Dialog_Structural_ManyBcDbar:Control_Structural_LineBc
//            ,Dialog_Structural_ManyBcDbar:Control_Structural_AreaBc
//            ,Dialog_Structural_ManyLoadsDbar:Control_Structural_PointLoad
//            ,Dialog_Structural_ManyLoadsDbar:Control_Structural_LineLoad
//            ,Dialog_Structural_ManyLoadsDbar:Control_Structural_AreaLoad
//            ,Dialog_Structural_ManyLoadsDbar:Control_Structural_AutoPointLoad
//            ,Dialog_Structural_ManyLoadsDbar:Control_Structural_AutoLineLoad
//            ,Dialog_Structural_ManyLoadsDbar:Control_Structural_AutoAreaLoad
//            ,Dialog_Structural_NewStructuralBeam:Control_Structural_CreateBeamOnGrid
//            ,Dialog_Structural_SpanSymbolPropDbar:Control_Structural_AlignSpanDir
//            ,Dialog_Structural_TagOnPlacementDlg:Control_Structural_TagOnPlacementCheckbox
//            ,Dialog_Structural_TrussEditProfileSketch:Control_Structural_EditTrussSketch
//            ,Dialog_Structural_TrussEditProfileSketchRem:Control_Structural_RemoveTrussSketch
//            ,Dialog_Structural_TrussEditProfileSketch:Control_Structural_ResetTruss
//            ,Dialog_Structural_TrussEditProfileSketch:Control_Structural_ExplodeTruss
//            ,Dialog_Structural_TrussEditProfileSketch:Control_Structural_AttachTrusses
//            ,Dialog_Structural_TrussEditProfileSketch:Control_Structural_DetachTrusses
//            ,Dialog_Structural_TrussEditProfileSketchRem:Control_Structural_EditTrussSketch
//            ,Dialog_Structural_TrussEditProfileSketchRem:Control_Structural_ResetTruss
//            ,Dialog_Structural_TrussEditProfileSketchRem:Control_Structural_ExplodeTruss
//            ,Dialog_Structural_TrussEditProfileSketchRem:Control_Structural_AttachTrusses
//            ,Dialog_Structural_TrussEditProfileSketchRem:Control_Structural_DetachTrusses
//            ,Dialog_Structural_TrussMultiSelect:Control_Structural_ResetTruss
//            ,Dialog_Structural_TrussMultiSelect:Control_Structural_ExplodeTrusses
//            ,Dialog_Structural_TrussMultiSelect:Control_Structural_AttachTrusses
//            ,Dialog_Structural_TrussMultiSelect:Control_Structural_DetachTrusses
//            ,Dialog_Structural_VerticalSlantedColumnsDialogBar:Control_Structural_VerticalColumns
//            ,Dialog_Structural_VerticalSlantedColumnsDialogBar:Control_Structural_SlantedColumns
            , ID_OBJECTS_GRID_CHAIN
//            ,IDD_COLUMN_ATTACH_DETACH:ID_ATTACH_COLUMNS
//            ,IDD_COLUMN_ATTACH_DETACH:ID_DETACH_COLUMNS
//            ,IDD_MULTIPICK_DBAR:ID_FINISH_MULTI_PICK_DBAR
//            ,IDD_MULTIPICK_DBAR:ID_CANCEL_MULTI_PICK_DBAR
//            ,IDD_MULTIPICK_DBAR:ID_SEL_MULTI_CHECK
//            ,IDD_WALL_ATTACH_DETACH:ID_EXTEND_WALLS
//            ,IDD_WALL_ATTACH_DETACH:ID_UNEXTEND_WALLS
//            ,Dialog_BuildingSystems_PanelScheduleLoadDlgbar:Control_BuildingSystems_CircuitsRebalanceLoads
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsMoveUp
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsMoveDown
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsMoveAcross
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsMoveTo
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsAssignSpare
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsAssignSpace
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsRemoveSpareSpace
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsLockUnlock
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsGroupUngroup
//            ,Dialog_BuildingSystems_PanelScheduleCircuitDlgbar:Control_BuildingSystems_CircuitsUpdateNames
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_EditFont
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_TextHorizontalAlignLeft
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_TextHorizontalAlignCenter
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_TextHorizontalAlignRight
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_TextVerticalAlignTop
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_TextVerticalAlignMiddle
//            ,Dialog_BuildingSystems_PanelScheduleTextDlgbar:Control_BuildingSystems_TextVerticalAlignBottom
//            ,Dialog_BuildingSystems_PanelScheduleSheetInstanceEditDlgBar:Control_BuildingSystems_SheetInstanceViewPanelSchedule
//            ,Dialog_Essentials_ConvertToModelLine:Control_Essentials_ConvertLines
//            ,Dialog_Essentials_ConvertToDetailLine:Control_Essentials_ConvertLines
//            ,Dialog_Essentials_ConvertToSymbolicLine:Control_Essentials_ConvertLines
//            ,Dialog_Essentials_ConvertMixedTypesLines:Control_Essentials_ConvertLines
//            ,Dialog_BuildingSystems_RbsInsulationLiningThickness:Control_BuildingSystems_AddDuctInsulation
//            ,Dialog_BuildingSystems_RbsInsulationLiningThickness:Control_BuildingSystems_EditDuctInsulation
//            ,Dialog_BuildingSystems_RbsInsulationLiningThickness:Control_BuildingSystems_RemoveDuctInsulation
//            ,Dialog_BuildingSystems_RbsInsulationLiningThickness:Control_BuildingSystems_AddDuctLining
//            ,Dialog_BuildingSystems_RbsInsulationLiningThickness:Control_BuildingSystems_EditDuctLining
//            ,Dialog_BuildingSystems_RbsInsulationLiningThickness:Control_BuildingSystems_RemoveDuctLining
//            ,Dialog_BuildingSystems_RbsPipeInsulationThickness:Control_BuildingSystems_AddPipeInsulation
//            ,Dialog_BuildingSystems_RbsPipeInsulationThickness:Control_BuildingSystems_EditPipeInsulation
//            ,Dialog_BuildingSystems_RbsPipeInsulationThickness:Control_BuildingSystems_RemovePipeInsulation
            , ID_RM_ASSEMBLY_EDITOR
            , ID_ASSEMBLY_DISASSEMBLE
            , ID_CREATE_SHOP_DRAWING
            , ID_ACQUIRE_ASSEMBLY_VIEWS
            , ID_FINISH_MULTI_PICK_DBAR
            , ID_CANCEL_MULTI_PICK_DBAR
            , ID_PARALLEL_PIPES_HORIZONTAL_NUMBER
            , ID_PARALLEL_PIPES_VERTICAL_NUMBER
            , ID_PARALLEL_PIPES_HORIZONTAL_OFFSET
            , ID_PARALLEL_PIPES_VERTICAL_OFFSET
            , ID_PARALLEL_CONDUITS_SAME_BEND_RADIUS
            , ID_PARALLEL_CONDUITS_RAINBOW_BEND_RADIUS
            , ID_PARALLEL_CONDUITS_HORIZONTAL_NUMBER
            , ID_PARALLEL_CONDUITS_VERTICAL_NUMBER
            , ID_PARALLEL_CONDUITS_HORIZONTAL_OFFSET
            , ID_PARALLEL_CONDUITS_VERTICAL_OFFSET
//            ,IDD_SHOW_ANALYTICAL_MODEL:IDC_SHOW_ANALYTICAL
//            ,IDD_SHOW_PHYSICAL_MODEL:IDC_SHOW_PHYSICAL
//            ,Dialog_ArrayElems_DividedPathSplitReferenceBar:Control_ArrayElems_DividedPathApplyLayout
//            ,Dialog_ArrayElems_DividedPathSplitReferenceBar:Control_ArrayElems_IntersectPick
//            ,Dialog_ArrayElems_DividedPathSplitReferenceBar:Control_ArrayElems_IntersectList
//            ,Dialog_ArrayElems_DividedPathDisplayPropertiesBar:Control_ArrayElems_DividedPathPath
//            ,Dialog_ArrayElems_DividedPathDisplayPropertiesBar:Control_ArrayElems_DividedPathComponent
//            ,ID_EDIT_REBAR_ELEMENT_SHAPE_FAMILY
            , ID_LOAD_BC_REHOST
//            ,Dialog_StairRamp_RailingPlaceOnHostDBar:Control_StairRamp_RailingPosTreads
//            ,Dialog_StairRamp_RailingPlaceOnHostDBar:Control_StairRamp_RailingPosStringer
//            ,Dialog_StairRamp_EditStairsElementDialogBar:Control_StairRamp_EditStairsElement
//            ,Dialog_StairRamp_UnsupportedObjectWarnDbar:Control_StairRamp_UnsupportedObjectWarn
            , ID_DISPLACEMENT_EDIT
            , ID_DISPLACEMENT_RESET
            , ID_DISPLACEMENT_PATH
//            ,Dialog_Rebar_FabricPlaceBendWireFrameDbar:Control_Rebar_FabricSketchBending
//            ,Dialog_Rebar_FabricRehostDialogBar:Control_Rebar_FabricRehost
            , ID_APP_EXIT
            , ID_OPTIONS
            , ID_FILE_NEW_CHOOSE_TEMPLATE
            , ID_FAMILY_NEW
            , ID_NEW_REVIT_DESIGN_MODEL
            , ID_TITLEBLOCK_NEW
            , ID_ANNOTATION_SYMBOL_NEW
            , ID_REVIT_FILE_OPEN
            , ID_APPMENU_PROJECT_OPEN
            , ID_FAMILY_OPEN
            , ID_OPEN_ADSK
            , ID_IMPORT_IFC
            , ID_IFC_IMPORT_OPTIONS
            , ID_SAMPLES_OPEN
            , ID_REVIT_FILE_SAVE
            , ID_REVIT_FILE_SAVE_AS
            , ID_REVIT_SAVE_AS_FAMILY
            , ID_REVIT_SAVE_AS_TEMPLATE
            , ID_SAVE_FAMILY_ANY
            , ID_SAVE_GROUP
            , ID_SAVE_VIEWS_TO_FILE
            , ID_EXPORT_DWG
            , ID_EXPORT_DXF
            , ID_EXPORT_DGN
            , ID_EXPORT_SAT
            , ID_EXPORT_DWF
            , ID_SITE_BUILDING_SITE_EXPORT
            , ID_EXPORT_FBX
            , ID_EXPORT_FAM_TYPES
            , ID_ABS_EXPORT_GBXML
            , ID_CEA_EXPORT_GBXML
            , ID_EXPORT_IFC
            , ID_FILE_EXPORT_ODBC
            , ID_RM_ANIMATION
            , ID_FILE_EXPORT_SOLAR_STUDY
            , ID_FILE_PRINT_TO_IMAGE
            , ID_FILE_EXPORT_SCHEDULE
            , ID_ROOM_AREA_REPORT
            , ID_SETTINGS_EXPORT_SETUPS_DWGDXF
            , ID_SETTINGS_EXPORT_SETUPS_DGN
            , ID_IFC_OPTIONS
            , ID_POSTTOBUZZSAW_DWF
            , ID_POSTTOBUZZSAW_DWG
            , ID_POSTTOBUZZSAW_DXF
            , ID_POSTTOBUZZSAW_DGN
            , ID_POSTTOBUZZSAW_SAT
            , ID_REVIT_FILE_PRINT
            , ID_REVIT_FILE_PRINT_PREVIEW
            , ID_REVIT_FILE_PRINT_SETUP
            , ID_REVIT_FILE_CLOSE
            , ID_BUTTON_UNDO
            , ID_BUTTON_REDO
            , ID_HELP_FINDER
            , ID_HELP_NEWS
            , ID_HELP_ESSENTIAL_VIDEOS
            , ID_HELP_LEARNING_PATHS
            , ID_HELP_VIDEOS
            , ID_HELP_SUPPORT_KNOWLEDGE_BASE
            , ID_HELP_WEBCONTENTLIBURL
            , ID_HELP_PRODUCTURL
            , ID_HELP_THIRDPARTYLEARNINGCONTENT
            , ID_HELP_DESKTOP_ANALYTICS
            , ID_HELP_CUSTOMERIMPROVEMENTPROGRAM
            , ID_APP_ABOUT
            , ID_OBJECTS_CURVE_ARC_TAN
            , ID_WALL_ARC_THREE_PNT
            , ID_OBJECTS_CURVE_ARC_THREE_PNT
            , ID_ORIENT_TO_BACK_RIGHT
            , ID_VIEWCUBE_HELP
            , ID_SNAP_OVERRIDE_END
            , ID_SET_FOCAL_LENGTH
            , IDC_RADIO_ARC_FILLET
            , ID_VIEWCUBE_TOGGLE_BETWEEN_PERSP_AND_ORTHO
            , ID_VIEWCUBE_RESET_FRONT
            , ID_WALL_STRUCT_POLY_CIRCUMSCRIBED
            , ID_VIEW_OVERRIDE_CATEGORY_GHOST
            , IDC_RADIO_FULL_ELLIPSE
            , ID_NAVWHEEL_SAVEVIEW
            , ID_IMAGE_INTERACTIVE_RAYTRACE
            , ID_TRUSS_LINE
            , ID_SNAP_OVERRIDE_NO_SNAP
            , ID_NAVWHEEL_DECREASEWALKSPEED
            , ID_WALL_STRUCT_ARC_THREE_PNT
            , ID_RESTORE_VIEW_PROPERTIES
            , ID_VIEWCUBE_LOCK_SELECTION
            , ID_SEL_ALL_IN_PRJ_IN_OPTION
            , ID_WALL_STRUCT_PICK_FACES
            , ID_IMAGE_HIDDENLINE
            , ID_SNAP_OVERRIDE_REMOTE
            , ID_WALL_STRUCT_ARC_FILLET
            , ID_OBJECTS_CURVE_ELLIPSE_PARTIAL
            , ID_LOCKUNLOCK3DVIEW_UNLOCK
            , IDC_RADIO_ARC_CENTER
            , IDC_RADIO_INSCRIBED_POLYGON
            , ID_NAVWHEEL_UNDOVIEWORIENTATIONCHANGES
            , ID_MOVE_CROP_BOUNDARY
            , ID_WALL_POLY_INSCRIBED
            , ID_SHOW_COMPASS
            , ID_MAKE_EDITABLE_PREFER_CHECKOUT
            , ID_DEFAULT_ROTATION_CENTER
            , IDC_RADIO_ARC_3_PNT
            , ID_GRAPHIC_DISPLAY_OPTIONS
            , ID_ELEM_GROUP_EXCLUDED_MEMBER
            , ID_WALL_STRUCT_ARC_TAN
            , ID_ORIENT_TO_OTHER_VIEW
            , ID_SNAP_OVERRIDE_CLOSE
            , ID_SELECT_IN_HOST_ALL_FABRIC
            , ID_VIEW_FRAME_TOGGLE_ANALYTICAL_MODEL_VISIBILITY
            , ID_TEMPHIDE_ISOLATE
            , ID_WALL_PICK_LINES
            , ID_LOCKUNLOCK3DVIEW_LOCKANDSAVE
            , ID_ORIENT_TO_TOP
            , ID_IMAGE_SHADING
            , ID_ORIENT_SOUTH_DIRECTION
            , ID_WALL_CIRCLE
            , ID_FILE_NEW_WITH_TEMPLATE4
            , ID_ENABLE_TEMPORARY_VIEW_PROPERTIES
            , ID_SETTINGS_SNAPS_NOOVERRIDE
            , IDC_RADIO_CIRCUMSCRIBED_POLYGON
            , ID_DETAIL_LEVEL_MEDIUM
            , ID_WALL_POLY_CIRCUMSCRIBED
            , ID_TEMPHIDE_ISOLATECAT
            , ID_SNAP_OVERRIDE_TANGENT
            , ID_SEL_ALL_IN_VIEW
            , ID_FILE_NEW_WITH_TEMPLATE3
            , ID_ZOOM_OUT_X2
            , ID_ORIENT_SOUTHEAST_DIRECTION
            , ID_NAVBAR_3DCONTROLLER_FLY
            , IDC_RADIO_CIRCLE
            , ID_VIEW_OVERRIDE_CATEGORY_TRANSPARENT
            , ID_NAVBAR_3DCONTROLLER_OBJECT
            , ID_OBJECTS_CURVE_POLY_CIRCUMSCRIBED
            , ID_SNAP_OVERRIDE_CENTER
            , ID_FIND_VIEW_GENERATOR
            , ID_ORIENT_NORTH_DIRECTION
            , Control_Revit_ExplodedViewMode
            , ID_SNAP_OVERRIDE_MID
            , ID_NAVWHEEL_LEVELCAMERA
            , ID_OBJECTS_CURVE_ARC_FILLET
            , ID_EXCLUDE_GROUP_MEMBERS
            , ID_IMAGE_FLAT_COLORS
            , ID_MAKE_EDITABLE_BY_BORROWING
            , ID_NAVWHEEL_MINI_EIGHT_WHEEL
            , ID_DETAIL_LEVEL_COARSE
            , ID_NAVWHEEL_CLOSE
            , ID_NAVWHEEL_OPTIONS
            , ID_NAVWHEEL_MINI_MINI_WHEEL
            , ID_VIEW_OVERRIDE_ELEMENTS_GHOST
            , ID_DETAIL_LEVEL_FINE
            , ID_TEMPORARILY_APPLY_TEMPLATE_PROPERTIES
            , ID_IMAGE_SHADOW_ON
            , ID_NAVWHEEL_INTERIOR_WHEEL
            , ID_VIEW_ZOOMANDPAN_NEXTVIEW
            , ID_IMAGE_WIREFRAME
            , ID_ORIENT_TO_LEFT
            , ID_GRID_ARC_CENTER_ENDS
            , ID_RUN_LINE
            , ID_WALL_ARC_CENTER_ENDS
            , ID_REPEAT_LASTCOMMAND
            , ID_WALL_STRUCT_LINE
            , ID_NAVWHEEL_MINI_OTHER_WHEEL
            , ID_FILE_NEW_WITH_TEMPLATE5
            , IDC_RADIO_PARTIAL_ELLIPSE
            , ID_TEMPHIDE_MAKEPERM
            , ID_NAVWHEEL_EXTERIOR_WHEEL
            , ID_IMAGE_REALISTIC
            , ID_OBJECTS_CURVE_RECT
            , IDC_RADIO_ARC_TAN_END
            , ID_ACTIVATE_CONTEXTUAL_TAB
            , ID_SKETCH_PICK_WALLS
            , ID_OBJECTS_CURVE_ARC_CENTER_ENDS
            , IDC_VIEW_FRAME_CROPREGION_ACTIVITY
            , ID_NAVBAR_3DCONTROLLER_WALK
            , ID_TEMPHIDE_RESET
            , ID_SNAP_OVERRIDE_QUADRANT
            , ID_SNAP_OVERRIDE_POINT_CLOUDS
            , ID_NAVWHEEL_RECENTERCROPBOUNDARY
            , ID_OBJECTS_CURVE_LINE
            , ID_ORIENT_NORTHEAST_DIRECTION
            , ID_ORIENT_WEST_DIRECTION
            , ID_ZOOM_ALL_ALL
            , ID_PAN_VIEW
            , ID_ORIENT_TO_RIGHT
            , ID_IMAGE_SHADING_WITH_EDGES
            , ID_GRID_ARC_THREE_PNT
            , ID_VIEW_OVERRIDE_ELEMENTS_HALFTONE
            , ID_ORIENT_NORTHWEST_DIRECTION
            , IDC_RADIO_RECT
            , ID_EDIT_SELECT_PREVIOUS
            , ID_VIEWCUBE_SET_HOME
            , ID_VIEWCUBE_HOME
            , ID_SNAP_OVERRIDE_SKETCH_GRID
            , ID_WALL_STRUCT_CIRCLE
            , ID_ORIENT_TOP_DIRECTION
            , ID_VIEWCUBE_OPTIONS
            , ID_SNAP_OVERRIDE_POINT
            , ID_TRUSS_PICK_LINES
            , ID_SELECT_IN_HOST_ALL_REBAR
            , ID_WALL_STRUCT_POLY_INSCRIBED
            , ID_ZOOM_FIT
            , ID_NAVBAR_3DCONTROLLER_2D
            , IDC_RADIO_LINE
            , ID_ZOOM_IN
            , ID_WALL_ARC_TAN
            , ID_WALL_STRUCT_ARC_CENTER_ENDS
            , ID_ORIENT_SOUTHWEST_DIRECTION
            , ID_WALL_RECT
            , ID_SEL_ALL_IN_PRJ
            , IDC_VIEW_FRAME_CROPREGION_VISIBILITY
            , ID_WALL_LINE
            , ID_TEMPHIDE_HIDECAT
            , ID_NAVWHEEL_FULL_WHEEL
            , ID_NAVWHEEL_INCREASEWALKSPEED
            , ID_OBJECTS_CURVE_ELLIPSE
            , ID_RUN_ARC_CENTER_ENDS
            , ID_FILE_NEW_WITH_TEMPLATE2
            , ID_NAVWHEEL_RESTOREORIGINALCENTER
            , ID_ZOOM_SHEET
            , ID_MOVE_GROUP_MEMBER_TO_PROJECT
            , ID_WALL_ARC_FILLET
            , ID_ORIENT_TO_FRONT_LEFT
            , ID_ORIENT_PLANE
            , ID_FILE_NEW_WITH_TEMPLATE1
            , ID_DEFINE_ROTATION_CENTER
            , ID_IMAGE_SHADOW_OFF
            , ID_GRID_LINE
            , ID_OBJECTS_CURVE_SPLINE
            , ID_NAVWHEEL_FITTOWINDOW
            , ID_ORIENT_TO_FRONT_RIGHT
            , ID_TEMPHIDE_HIDE
            , ID_OBJECTS_CURVE_CIRCLE
            , ID_TOGGLE_REVEAL_CONSTRAINTS
            , ID_NAVWHEEL_HELP
            , ID_ORIENT_TO_BACK
            , ID_VIEW_ZOOMANDPAN_PREVIOUSVIEW
            , ID_SET_FRONT_TO_CURRENT_VIEW
            , ID_VIEW_OVERRIDE_ELEMENTS_TRANSPARENT
            , IDC_RADIO_COPY_CURVE
            , IDC_RADIO_SPLINE
            , ID_SHOW_PROPERTIES_PALETTE
            , ID_OBJECTS_CURVE_POLY_INSCRIBED
            , ID_ORIENT_TO_FRONT
            , ID_SNAP_OVERRIDE_INTERSECT
            , ID_VIEW_OVERRIDE_CATEGORY_HALFTONE
            , ID_SEL_ALL_IN_VIEW_IN_OPTION
            , ID_SELECT_IN_HOST_ALL_REINFORCEMENT
            , ID_WALL_STRUCT_RECT
            , ID_GRID_PICK_LINES
            , ID_ORIENT_EAST_DIRECTION
            , ID_LOCKUNLOCK3DVIEW_LOCKANDRESTORE
            , ID_SNAP_OVERRIDE_INTERIOR
            , ID_OBJECTS_CURVE_PICK_LINES
            , ID_ORIENT_TO_BACK_LEFT
            , ID_WALL_STRUCT_PICK_LINES
            , ID_SNAP_OVERRIDE_PERP

        }

    }
}
