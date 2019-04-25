
using System;
using System.Collections.Generic;
using System.Text;

namespace BinLibrary.RevitRibbonHelper
{

    public enum RvtTabIds
    {
        Architecture,//建筑
        Structure,//结构
        Steel,//钢
        Systems,//系统
        Insert,//插入
        Annotate,//注释
        Analyze,//分析
        MassingSite,//体量和场地
        Collaborate,//协作
        View,//视图
        Manage,//管理
        Home_Family,//创建
        Insert_AnnotationDetailModelMassConceptualProfileTrussFamily,//插入
        Annotate_ModelMassFamily,//注释
        View_Family,//视图
        //Add-Ins,//附加模块
        Manage_Family,//管理
        //未来科技公司工具集,//未来科技公司工具集
        Modify,//修改
        Second_Modify,//修改
        InPlaceModelFamilyTab,//内建模型
        InPlaceMassFamilyTab,//内建体量
        FamilyEditorTab,//族编辑器
    }
    public enum PanelArchitecture
    {//建筑
        selection_shr,//选择
        build_rac,//构建
        circulation_shr,//楼梯坡道
        basicmodel_shr,//模型
        roomarea_shr,//房间和面积
        opening_shr,//洞口
        datum_shr,//基准
        workplane_shr,//工作平面
    }
    public enum PanelStructure
    {//结构
        selection_shr,//选择
        structure_shr,//结构
        structure_conn,//连接
        foundation_shr,//基础
        rebar_rst,//钢筋
        model_rst,//模型
        opening_rst,//洞口
        datum_shr,//基准
        workplane_shr,//工作平面
        structdet_shr //连接工具
    }

    public enum PanelSteel
    {//钢
        selection_shr,//选择
        structure_conn,//连接
        ext_entity_fabrication,//预制图元
        ext_entity_modifiers,//修改器
        ext_entity_parametric_cuts,//参数化切割
        workplane_shr,//工作平面
        ext_entity_esf_settings,//设置
    }
    public enum PanelSystems
    {//系统
        selection_shr,//选择
        hvac_rme,//HVAC
        mep_fabrication,//预制
        mep_pnid_collaboration,//P&ID 协作
        mechequip_rme,//机械
        mechequip_elec_rme,//机械  mechequip-elec_rme
        piping_rme,//卫浴和管道
        electrical_rme,//电气
        model_rme,//模型
        workplane_shr,//工作平面
    }
    public enum PanelInsert
    {//插入
        selection_shr,//选择
        link_shr,//链接
        import_shr,//导入
        load_from_library_shr,//从库中载入
    }
    public enum PanelAnnotate
    {//注释
        selection_shr,//选择
        dimension_shr,//尺寸标注
        detail_shr,//详图
        text_shr,//文字
        tag_shr,//标记
        colorschemes_rme,//颜色填充
        symbol_shr,//符号
    }
    public enum PanelAnalyze
    {//分析
        selection_shr,//选择
        analyticalmodel_rst,//分析模型
        analyticaltools_rst,//分析模型工具
        spaces_rme,//空间和分区
        reports_rme,//报告和明细表
        checksystems_rme,//检查系统
        colorschemes_rme,//颜色填充
        cea_shr,//能量分析
    }
    public enum PanelMassingSite
    {//体量和场地
        selection_shr,//选择
        massing_shr,//概念体量
        massing_face_shr,//面模型
        sitemodeling_shr,//场地建模
        sitemodify_shr,//修改场地
        workflow_shr,//工作流程
    }
    public enum PanelCollaborate
    {//协作
        selection_shr,//选择
        worksharing_communicate_shr,//通信
        worksharing_shr,//管理协作
        central_file_shr,//同步
        worksharing_administer_shr,//管理模型
        coordination_shr,//坐标
    }
    public enum PanelView
    {//视图
        selection_shr,//选择
        viewgraphics_shr,//图形
        viewpresentation_shr,//演示视图
        createview_shr,//创建
        sheets_shr,//图纸组合
        manageviews_shr,//窗口
    }
    public enum PanelManage
    {//管理
        selection_shr,//选择
        projectlocation_shr,//项目位置
        designoptions_shr,//设计选项
        manageproject_shr,//管理项目
        phasing_shr,//阶段化
        settings_shr,//设置
        selectionsets_shr,//选择
        inquiry_shr,//查询
        addindevelopment_shr,//宏
        visualprogramming_shr,//可视化编程
    }

    public enum PanelHome_Family
    {//创建
        selection_shr,//选择
        properties_shr,//属性
        rebarlines_fam,//绘制
        detail_detail_fam,//详图
        detail_profile_fam,//详图
        detail_truss_fam,//详图
        draw_michelangelo,//绘制
        workplane_michelangelo,//工作平面
        model_michelangelo_fam,//模型
        dimension_shr,//尺寸标注
        text_shr,//文字
        forms_fam,//形状
        model_fam,//模型
        control_fam,//控件
        connectors_fam,//连接件
        datum_shr,//基准
        datum_fam_michelangelo,//基准
        workplane_shr,//工作平面
        rebartypes_fam,//钢筋类型
    }
    public enum PanelInsert_AnnotationDetailModelMassConceptualProfileTrussFamily
    {//插入
        selection_shr,//选择
        link_shr,//链接
        import_shr,//导入
        load_from_library_shr,//从库中载入
    }
    public enum PanelAnnotate_ModelMassFamily
    {//注释
        selection_shr,//选择
        dimension_shr,//尺寸标注
        detail_fam,//详图
        text_shr,//文字
    }
    public enum PanelView_Family
    {//视图
        selection_shr,//选择
        graphics_fam,//图形
        createview_shr,//创建
        manageviews_shr,//窗口
    }
    //public enum PanelAdd_Ins
    //{//附加模块
    //    selection_shr,//选择
    //    EXTERNAL_TOOLS_PANEL,//外部
    //    CustomCtrl_%Add-Ins%Batch Print,//Batch Print
    //    CustomCtrl_%Add-Ins%eTransmit,//eTransmit
    //    CustomCtrl_%Add-Ins%Model Review,//Model Review
    //    CustomCtrl_%Add-Ins%WorksharingMonitor,//WorksharingMonitor
    //    CustomCtrl_%Add-Ins%FormIt Converter,//FormIt Converter
    //    CustomCtrl_%Add-Ins%Revit Lookup,//Revit Lookup
    //    CustomCtrl_%Add-Ins%Site,//Site
    //}
    public enum PanelManage_Family
    {//管理
        selection_shr,//选择
        settings_shr,//设置
        projectlocation_fam_michelangelo,//项目位置
        manageproject_shr,//管理项目
        inquiry_shr,//查询
        addindevelopment_shr,//宏
        visualprogramming_shr,//可视化编程
    }
    //public enum Panel未来科技公司工具集
    //{//未来科技公司工具集
    //    CustomCtrl_%未来科技公司工具集%开洞,//开洞
    //    CustomCtrl_%未来科技公司工具集%导出STL,//导出STL
    //    CustomCtrl_%未来科技公司工具集%管线修改,//管线修改
    //}
    public enum PanelModify
    {//修改
        clipboard_shr,//剪贴板
        geometry_shr,//几何图形
        modify_shr,//修改
        view_shr,//视图
        dimension_modify_shr,//测量
        create_shr,//创建
        selection_shr,//选择
        draw_michelangelo,//绘制
        rebarlines_fam,//绘制
        TypeSelectorPanel,//类型选择器
        properties_shr,//属性
        workplane_michelangelo,//工作平面
    }
    public enum PanelSecond_Modify
    {//修改
        selection_shr,//选择
        properties_shr,//属性
        TypeSelectorPanel,//类型选择器
        rebarlines_fam,//绘制
        draw_michelangelo,//绘制
        workplane_michelangelo,//工作平面
        clipboard_shr,//剪贴板
        geometry_shr,//几何图形
        modify_shr,//修改
        view_shr,//视图
        dimension_modify_shr,//测量
        create_shr,//创建
    }
    public enum PanelInPlaceModelFamilyTab
    {//内建模型
        inplace_mode_shr,//在位编辑器
    }
    public enum PanelInPlaceMassFamilyTab
    {//内建体量
        inplace_mode_mass_shr,//在位编辑器
    }
    public enum PanelFamilyEditorTab
    {//族编辑器
        familyeditor_mode_shr,//族编辑器
    }
}