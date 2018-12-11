Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraScheduler.UI
Namespace Scheduler13._1_TestDesignTime
	Partial Public Class AppointmentFormOutlook2007Style
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AppointmentFormOutlook2007Style))
            Dim OptionsSpelling1 As DevExpress.XtraSpellChecker.OptionsSpelling = New DevExpress.XtraSpellChecker.OptionsSpelling()
            Dim OptionsSpelling2 As DevExpress.XtraSpellChecker.OptionsSpelling = New DevExpress.XtraSpellChecker.OptionsSpelling()
            Dim OptionsSpelling3 As DevExpress.XtraSpellChecker.OptionsSpelling = New DevExpress.XtraSpellChecker.OptionsSpelling()
            Me.ribPageAppointment = New DevExpress.XtraBars.Ribbon.RibbonPage()
            Me.pgrActions = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            Me.btnSaveAndClose = New DevExpress.XtraBars.BarButtonItem()
            Me.btnDelete = New DevExpress.XtraBars.BarButtonItem()
            Me.pgrOptions = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            Me.barEditShowTimeAs = New DevExpress.XtraBars.BarEditItem()
            Me.repItemAppointmentStatus = New DevExpress.XtraScheduler.UI.RepositoryItemAppointmentStatus()
            Me.barEditLabelAs = New DevExpress.XtraBars.BarEditItem()
            Me.repItemAppointmentLabel = New DevExpress.XtraScheduler.UI.RepositoryItemAppointmentLabel()
            Me.btnRecurrence = New DevExpress.XtraBars.BarButtonItem()
            Me.pgrProofing = New DevExpress.XtraBars.Ribbon.RibbonPageGroup()
            Me.btnSpelling = New DevExpress.XtraBars.BarButtonItem()
            Me.btnClose = New DevExpress.XtraBars.BarButtonItem()
            Me.btnSave = New DevExpress.XtraBars.BarButtonItem()
            Me.ribbonControl = New DevExpress.XtraBars.Ribbon.RibbonControl()
            Me.applicationMenu = New DevExpress.XtraBars.Ribbon.ApplicationMenu(Me.components)
            Me.barAndDockingController = New DevExpress.XtraBars.BarAndDockingController(Me.components)
            Me.spellChecker = New DevExpress.XtraSpellChecker.SpellChecker(Me.components)
            Me.tbLocation = New DevExpress.XtraEditors.TextEdit()
            Me.layoutCtrl = New DevExpress.XtraLayout.LayoutControl()
            Me.SearchLookUpEdit1 = New DevExpress.XtraEditors.SearchLookUpEdit()
            Me.SearchLookUpEdit1View = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.edtResources = New DevExpress.XtraScheduler.UI.AppointmentResourcesEdit()
            Me.lblHorzSeparator1 = New DevExpress.XtraEditors.LabelControl()
            Me.lblInfo = New DevExpress.XtraEditors.LabelControl()
            Me.lblHorzSeparator2 = New DevExpress.XtraEditors.LabelControl()
            Me.tbSubject = New DevExpress.XtraEditors.TextEdit()
            Me.edtStartDate = New DevExpress.XtraEditors.DateEdit()
            Me.cbReminder = New DevExpress.XtraScheduler.UI.DurationEdit()
            Me.edtResource = New DevExpress.XtraScheduler.UI.AppointmentResourceEdit()
            Me.edtEndDate = New DevExpress.XtraEditors.DateEdit()
            Me.tbDescription = New DevExpress.XtraEditors.MemoEdit()
            Me.edtStartTime = New DevExpress.XtraScheduler.UI.SchedulerTimeEdit()
            Me.edtEndTime = New DevExpress.XtraScheduler.UI.SchedulerTimeEdit()
            Me.chkAllDay = New DevExpress.XtraEditors.CheckEdit()
            Me.chkReminder = New DevExpress.XtraEditors.CheckEdit()
            Me.layoutControlGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutLocation = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutSubject = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutDescription = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutAllDay = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutInfo = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutReminderGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutChkReminder = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutReminder = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutStartGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutStartDate = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutStartTime = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutEndGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutEndDate = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutEndTime = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutResourcesGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.layoutResources = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutResource = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutHorzSeparator1 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.layoutHorzSeparator2 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.emptySpaceItem1 = New DevExpress.XtraLayout.EmptySpaceItem()
            Me.LayoutControlItem3 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.LayoutControlItem1 = New DevExpress.XtraLayout.LayoutControlItem()
            Me.BarEditItem2 = New DevExpress.XtraBars.BarEditItem()
            Me.BarEditItem1 = New DevExpress.XtraBars.BarEditItem()
            Me.BarEditItem3 = New DevExpress.XtraBars.BarEditItem()
            CType(Me.repItemAppointmentStatus, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.repItemAppointmentLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ribbonControl, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.applicationMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.barAndDockingController, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.tbLocation.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutCtrl, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.layoutCtrl.SuspendLayout()
            CType(Me.SearchLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.SearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtResources.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtResources.ResourcesCheckedListBoxControl, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.tbSubject.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtStartDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtStartDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.cbReminder.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtResource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtEndDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtEndDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.tbDescription.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtStartTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.edtEndTime.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkAllDay.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkReminder.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutControlGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutLocation, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutSubject, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutAllDay, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutInfo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutReminderGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutChkReminder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutReminder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutStartGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutStartTime, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutEndGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutEndTime, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutResourcesGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutResources, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutResource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutHorzSeparator1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.layoutHorzSeparator2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ribPageAppointment
            '
            Me.ribPageAppointment.Groups.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPageGroup() {Me.pgrActions, Me.pgrOptions, Me.pgrProofing})
            Me.ribPageAppointment.KeyTip = "AP"
            Me.ribPageAppointment.Name = "ribPageAppointment"
            resources.ApplyResources(Me.ribPageAppointment, "ribPageAppointment")
            '
            'pgrActions
            '
            Me.pgrActions.ItemLinks.Add(Me.btnSaveAndClose)
            Me.pgrActions.ItemLinks.Add(Me.btnDelete)
            Me.pgrActions.KeyTip = "A"
            Me.pgrActions.Name = "pgrActions"
            Me.pgrActions.ShowCaptionButton = False
            resources.ApplyResources(Me.pgrActions, "pgrActions")
            '
            'btnSaveAndClose
            '
            resources.ApplyResources(Me.btnSaveAndClose, "btnSaveAndClose")
            Me.btnSaveAndClose.Id = 4
            Me.btnSaveAndClose.Name = "btnSaveAndClose"
            Me.btnSaveAndClose.RibbonStyle = CType(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) _
                Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
            '
            'btnDelete
            '
            resources.ApplyResources(Me.btnDelete, "btnDelete")
            Me.btnDelete.Id = 5
            Me.btnDelete.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D))
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.RibbonStyle = CType(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) _
                Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
            '
            'pgrOptions
            '
            Me.pgrOptions.ItemLinks.Add(Me.barEditShowTimeAs)
            Me.pgrOptions.ItemLinks.Add(Me.barEditLabelAs)
            Me.pgrOptions.ItemLinks.Add(Me.btnRecurrence)
            Me.pgrOptions.KeyTip = "O"
            Me.pgrOptions.Name = "pgrOptions"
            Me.pgrOptions.ShowCaptionButton = False
            resources.ApplyResources(Me.pgrOptions, "pgrOptions")
            '
            'barEditShowTimeAs
            '
            resources.ApplyResources(Me.barEditShowTimeAs, "barEditShowTimeAs")
            Me.barEditShowTimeAs.Edit = Me.repItemAppointmentStatus
            Me.barEditShowTimeAs.Id = 10
            Me.barEditShowTimeAs.Name = "barEditShowTimeAs"
            '
            'repItemAppointmentStatus
            '
            resources.ApplyResources(Me.repItemAppointmentStatus, "repItemAppointmentStatus")
            Me.repItemAppointmentStatus.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("repItemAppointmentStatus.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.repItemAppointmentStatus.Name = "repItemAppointmentStatus"
            '
            'barEditLabelAs
            '
            resources.ApplyResources(Me.barEditLabelAs, "barEditLabelAs")
            Me.barEditLabelAs.Edit = Me.repItemAppointmentLabel
            Me.barEditLabelAs.Id = 11
            Me.barEditLabelAs.Name = "barEditLabelAs"
            '
            'repItemAppointmentLabel
            '
            resources.ApplyResources(Me.repItemAppointmentLabel, "repItemAppointmentLabel")
            Me.repItemAppointmentLabel.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("repItemAppointmentLabel.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.repItemAppointmentLabel.Name = "repItemAppointmentLabel"
            '
            'btnRecurrence
            '
            resources.ApplyResources(Me.btnRecurrence, "btnRecurrence")
            Me.btnRecurrence.Id = 6
            Me.btnRecurrence.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.G))
            Me.btnRecurrence.Name = "btnRecurrence"
            Me.btnRecurrence.RibbonStyle = CType(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) _
                Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
            '
            'pgrProofing
            '
            Me.pgrProofing.ItemLinks.Add(Me.btnSpelling)
            Me.pgrProofing.KeyTip = "P"
            Me.pgrProofing.Name = "pgrProofing"
            Me.pgrProofing.ShowCaptionButton = False
            resources.ApplyResources(Me.pgrProofing, "pgrProofing")
            '
            'btnSpelling
            '
            resources.ApplyResources(Me.btnSpelling, "btnSpelling")
            Me.btnSpelling.Id = 7
            Me.btnSpelling.ItemShortcut = New DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F7)
            Me.btnSpelling.Name = "btnSpelling"
            Me.btnSpelling.RibbonStyle = CType(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) _
                Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
            '
            'btnClose
            '
            resources.ApplyResources(Me.btnClose, "btnClose")
            Me.btnClose.Id = 3
            Me.btnClose.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4))
            Me.btnClose.Name = "btnClose"
            Me.btnClose.RibbonStyle = CType(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) _
                Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
            '
            'btnSave
            '
            resources.ApplyResources(Me.btnSave, "btnSave")
            Me.btnSave.Id = 2
            Me.btnSave.ItemShortcut = New DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S))
            Me.btnSave.Name = "btnSave"
            Me.btnSave.RibbonStyle = CType(((DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText) _
                Or DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithoutText), DevExpress.XtraBars.Ribbon.RibbonItemStyles)
            '
            'ribbonControl
            '
            resources.ApplyResources(Me.ribbonControl, "ribbonControl")
            Me.ribbonControl.AllowMdiChildButtons = False
            Me.ribbonControl.AllowMinimizeRibbon = False
            Me.ribbonControl.ApplicationButtonDropDownControl = Me.applicationMenu
            Me.ribbonControl.AutoSizeItems = True
            Me.ribbonControl.Controller = Me.barAndDockingController
            Me.ribbonControl.ExpandCollapseItem.Id = 0
            Me.ribbonControl.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.ribbonControl.ExpandCollapseItem, Me.btnSave, Me.btnClose, Me.btnSaveAndClose, Me.btnDelete, Me.btnRecurrence, Me.btnSpelling, Me.barEditShowTimeAs, Me.barEditLabelAs})
            Me.ribbonControl.ItemsVertAlign = DevExpress.Utils.VertAlignment.Top
            Me.ribbonControl.MaxItemId = 1
            Me.ribbonControl.Name = "ribbonControl"
            Me.ribbonControl.Pages.AddRange(New DevExpress.XtraBars.Ribbon.RibbonPage() {Me.ribPageAppointment})
            Me.ribbonControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repItemAppointmentStatus, Me.repItemAppointmentLabel})
            Me.ribbonControl.ShowToolbarCustomizeItem = False
            Me.ribbonControl.Toolbar.ItemLinks.Add(Me.btnSave)
            Me.ribbonControl.Toolbar.ShowCustomizeItem = False
            Me.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above
            '
            'applicationMenu
            '
            Me.applicationMenu.ItemLinks.Add(Me.btnSave)
            Me.applicationMenu.ItemLinks.Add(Me.btnDelete)
            Me.applicationMenu.ItemLinks.Add(Me.btnClose)
            Me.applicationMenu.MenuDrawMode = DevExpress.XtraBars.MenuDrawMode.LargeImagesText
            Me.applicationMenu.Name = "applicationMenu"
            Me.applicationMenu.Ribbon = Me.ribbonControl
            '
            'barAndDockingController
            '
            Me.barAndDockingController.PropertiesBar.AllowLinkLighting = False
            Me.barAndDockingController.PropertiesBar.DefaultGlyphSize = New System.Drawing.Size(16, 16)
            Me.barAndDockingController.PropertiesBar.DefaultLargeGlyphSize = New System.Drawing.Size(32, 32)
            '
            'spellChecker
            '
            Me.spellChecker.CheckAsYouTypeOptions.CheckControlsInParentContainer = True
            Me.spellChecker.Culture = New System.Globalization.CultureInfo("en-US")
            Me.spellChecker.ParentContainer = Me
            Me.spellChecker.SpellCheckMode = DevExpress.XtraSpellChecker.SpellCheckMode.AsYouType
            '
            'tbLocation
            '
            resources.ApplyResources(Me.tbLocation, "tbLocation")
            Me.tbLocation.Name = "tbLocation"
            Me.spellChecker.SetShowSpellCheckMenu(Me.tbLocation, True)
            Me.spellChecker.SetSpellCheckerOptions(Me.tbLocation, OptionsSpelling1)
            Me.tbLocation.StyleController = Me.layoutCtrl
            '
            'layoutCtrl
            '
            Me.layoutCtrl.Controls.Add(Me.SearchLookUpEdit1)
            Me.layoutCtrl.Controls.Add(Me.edtResources)
            Me.layoutCtrl.Controls.Add(Me.tbLocation)
            Me.layoutCtrl.Controls.Add(Me.lblHorzSeparator1)
            Me.layoutCtrl.Controls.Add(Me.lblInfo)
            Me.layoutCtrl.Controls.Add(Me.lblHorzSeparator2)
            Me.layoutCtrl.Controls.Add(Me.tbSubject)
            Me.layoutCtrl.Controls.Add(Me.edtStartDate)
            Me.layoutCtrl.Controls.Add(Me.cbReminder)
            Me.layoutCtrl.Controls.Add(Me.edtResource)
            Me.layoutCtrl.Controls.Add(Me.edtEndDate)
            Me.layoutCtrl.Controls.Add(Me.tbDescription)
            Me.layoutCtrl.Controls.Add(Me.edtStartTime)
            Me.layoutCtrl.Controls.Add(Me.edtEndTime)
            Me.layoutCtrl.Controls.Add(Me.chkAllDay)
            Me.layoutCtrl.Controls.Add(Me.chkReminder)
            resources.ApplyResources(Me.layoutCtrl, "layoutCtrl")
            Me.layoutCtrl.Name = "layoutCtrl"
            Me.layoutCtrl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = New System.Drawing.Rectangle(410, 412, 627, 466)
            Me.layoutCtrl.Root = Me.layoutControlGroup
            '
            'SearchLookUpEdit1
            '
            resources.ApplyResources(Me.SearchLookUpEdit1, "SearchLookUpEdit1")
            Me.SearchLookUpEdit1.MenuManager = Me.ribbonControl
            Me.SearchLookUpEdit1.Name = "SearchLookUpEdit1"
            Me.SearchLookUpEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("SearchLookUpEdit1.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.SearchLookUpEdit1.Properties.View = Me.SearchLookUpEdit1View
            Me.SearchLookUpEdit1.StyleController = Me.layoutCtrl
            '
            'SearchLookUpEdit1View
            '
            Me.SearchLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
            Me.SearchLookUpEdit1View.Name = "SearchLookUpEdit1View"
            Me.SearchLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = False
            Me.SearchLookUpEdit1View.OptionsView.ShowGroupPanel = False
            '
            'edtResources
            '
            resources.ApplyResources(Me.edtResources, "edtResources")
            Me.edtResources.Name = "edtResources"
            Me.edtResources.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("edtResources.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            '
            '
            '
            Me.edtResources.ResourcesCheckedListBoxControl.Location = CType(resources.GetObject("edtResources.ResourcesCheckedListBoxControl.Location"), System.Drawing.Point)
            Me.edtResources.ResourcesCheckedListBoxControl.Name = ""
            Me.edtResources.ResourcesCheckedListBoxControl.TabIndex = CType(resources.GetObject("edtResources.ResourcesCheckedListBoxControl.TabIndex"), Integer)
            Me.edtResources.StyleController = Me.layoutCtrl
            '
            'lblHorzSeparator1
            '
            Me.lblHorzSeparator1.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal
            Me.lblHorzSeparator1.LineVisible = True
            resources.ApplyResources(Me.lblHorzSeparator1, "lblHorzSeparator1")
            Me.lblHorzSeparator1.Name = "lblHorzSeparator1"
            Me.lblHorzSeparator1.StyleController = Me.layoutCtrl
            '
            'lblInfo
            '
            Me.lblInfo.Appearance.BackColor = CType(resources.GetObject("lblInfo.Appearance.BackColor"), System.Drawing.Color)
            resources.ApplyResources(Me.lblInfo, "lblInfo")
            Me.lblInfo.Name = "lblInfo"
            Me.lblInfo.StyleController = Me.layoutCtrl
            '
            'lblHorzSeparator2
            '
            Me.lblHorzSeparator2.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal
            Me.lblHorzSeparator2.LineVisible = True
            resources.ApplyResources(Me.lblHorzSeparator2, "lblHorzSeparator2")
            Me.lblHorzSeparator2.Name = "lblHorzSeparator2"
            Me.lblHorzSeparator2.StyleController = Me.layoutCtrl
            '
            'tbSubject
            '
            resources.ApplyResources(Me.tbSubject, "tbSubject")
            Me.tbSubject.Name = "tbSubject"
            Me.spellChecker.SetShowSpellCheckMenu(Me.tbSubject, True)
            Me.spellChecker.SetSpellCheckerOptions(Me.tbSubject, OptionsSpelling2)
            Me.tbSubject.StyleController = Me.layoutCtrl
            '
            'edtStartDate
            '
            resources.ApplyResources(Me.edtStartDate, "edtStartDate")
            Me.edtStartDate.Name = "edtStartDate"
            Me.edtStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("edtStartDate.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.edtStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.edtStartDate.StyleController = Me.layoutCtrl
            '
            'cbReminder
            '
            resources.ApplyResources(Me.cbReminder, "cbReminder")
            Me.cbReminder.Name = "cbReminder"
            Me.cbReminder.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("cbReminder.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.cbReminder.Properties.ShowEmptyItem = False
            Me.cbReminder.StyleController = Me.layoutCtrl
            '
            'edtResource
            '
            resources.ApplyResources(Me.edtResource, "edtResource")
            Me.edtResource.Name = "edtResource"
            Me.edtResource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("edtResource.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.edtResource.StyleController = Me.layoutCtrl
            '
            'edtEndDate
            '
            resources.ApplyResources(Me.edtEndDate, "edtEndDate")
            Me.edtEndDate.Name = "edtEndDate"
            Me.edtEndDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(CType(resources.GetObject("edtEndDate.Properties.Buttons"), DevExpress.XtraEditors.Controls.ButtonPredefines))})
            Me.edtEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.edtEndDate.StyleController = Me.layoutCtrl
            '
            'tbDescription
            '
            resources.ApplyResources(Me.tbDescription, "tbDescription")
            Me.tbDescription.Name = "tbDescription"
            Me.spellChecker.SetShowSpellCheckMenu(Me.tbDescription, True)
            Me.spellChecker.SetSpellCheckerOptions(Me.tbDescription, OptionsSpelling3)
            Me.tbDescription.StyleController = Me.layoutCtrl
            Me.tbDescription.UseOptimizedRendering = True
            '
            'edtStartTime
            '
            resources.ApplyResources(Me.edtStartTime, "edtStartTime")
            Me.edtStartTime.Name = "edtStartTime"
            Me.edtStartTime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.edtStartTime.StyleController = Me.layoutCtrl
            '
            'edtEndTime
            '
            resources.ApplyResources(Me.edtEndTime, "edtEndTime")
            Me.edtEndTime.Name = "edtEndTime"
            Me.edtEndTime.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.edtEndTime.StyleController = Me.layoutCtrl
            '
            'chkAllDay
            '
            resources.ApplyResources(Me.chkAllDay, "chkAllDay")
            Me.chkAllDay.Name = "chkAllDay"
            Me.chkAllDay.Properties.Caption = resources.GetString("chkAllDay.Properties.Caption")
            Me.chkAllDay.StyleController = Me.layoutCtrl
            '
            'chkReminder
            '
            resources.ApplyResources(Me.chkReminder, "chkReminder")
            Me.chkReminder.Name = "chkReminder"
            Me.chkReminder.Properties.Caption = resources.GetString("chkReminder.Properties.Caption")
            Me.chkReminder.StyleController = Me.layoutCtrl
            '
            'layoutControlGroup
            '
            resources.ApplyResources(Me.layoutControlGroup, "layoutControlGroup")
            Me.layoutControlGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.layoutLocation, Me.layoutSubject, Me.layoutDescription, Me.layoutAllDay, Me.layoutInfo, Me.layoutReminderGroup, Me.layoutStartGroup, Me.layoutEndGroup, Me.layoutResourcesGroup, Me.layoutHorzSeparator1, Me.layoutHorzSeparator2, Me.emptySpaceItem1, Me.LayoutControlItem3})
            Me.layoutControlGroup.Location = New System.Drawing.Point(0, 0)
            Me.layoutControlGroup.Name = "Root"
            Me.layoutControlGroup.Size = New System.Drawing.Size(609, 449)
            Me.layoutControlGroup.TextVisible = False
            '
            'layoutLocation
            '
            Me.layoutLocation.Control = Me.tbLocation
            resources.ApplyResources(Me.layoutLocation, "layoutLocation")
            Me.layoutLocation.Location = New System.Drawing.Point(0, 106)
            Me.layoutLocation.Name = "layoutLocation"
            Me.layoutLocation.Size = New System.Drawing.Size(589, 24)
            Me.layoutLocation.TextSize = New System.Drawing.Size(88, 13)
            '
            'layoutSubject
            '
            Me.layoutSubject.Control = Me.tbSubject
            resources.ApplyResources(Me.layoutSubject, "layoutSubject")
            Me.layoutSubject.Location = New System.Drawing.Point(0, 82)
            Me.layoutSubject.Name = "layoutSubject"
            Me.layoutSubject.Size = New System.Drawing.Size(589, 24)
            Me.layoutSubject.TextSize = New System.Drawing.Size(88, 13)
            '
            'layoutDescription
            '
            Me.layoutDescription.Control = Me.tbDescription
            resources.ApplyResources(Me.layoutDescription, "layoutDescription")
            Me.layoutDescription.Location = New System.Drawing.Point(0, 260)
            Me.layoutDescription.Name = "layoutDescription"
            Me.layoutDescription.Size = New System.Drawing.Size(589, 169)
            Me.layoutDescription.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutDescription.TextToControlDistance = 0
            Me.layoutDescription.TextVisible = False
            '
            'layoutAllDay
            '
            Me.layoutAllDay.Control = Me.chkAllDay
            resources.ApplyResources(Me.layoutAllDay, "layoutAllDay")
            Me.layoutAllDay.Location = New System.Drawing.Point(339, 195)
            Me.layoutAllDay.Name = "layoutAllDay"
            Me.layoutAllDay.Size = New System.Drawing.Size(102, 24)
            Me.layoutAllDay.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutAllDay.TextToControlDistance = 0
            Me.layoutAllDay.TextVisible = False
            '
            'layoutInfo
            '
            Me.layoutInfo.Control = Me.lblInfo
            resources.ApplyResources(Me.layoutInfo, "layoutInfo")
            Me.layoutInfo.Location = New System.Drawing.Point(0, 0)
            Me.layoutInfo.Name = "layoutInfo"
            Me.layoutInfo.Size = New System.Drawing.Size(589, 58)
            Me.layoutInfo.TextLocation = DevExpress.Utils.Locations.Top
            Me.layoutInfo.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutInfo.TextToControlDistance = 0
            Me.layoutInfo.TextVisible = False
            '
            'layoutReminderGroup
            '
            resources.ApplyResources(Me.layoutReminderGroup, "layoutReminderGroup")
            Me.layoutReminderGroup.GroupBordersVisible = False
            Me.layoutReminderGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.layoutChkReminder, Me.layoutReminder})
            Me.layoutReminderGroup.Location = New System.Drawing.Point(0, 236)
            Me.layoutReminderGroup.Name = "layoutReminderGroup"
            Me.layoutReminderGroup.Size = New System.Drawing.Size(589, 24)
            '
            'layoutChkReminder
            '
            Me.layoutChkReminder.Control = Me.chkReminder
            resources.ApplyResources(Me.layoutChkReminder, "layoutChkReminder")
            Me.layoutChkReminder.Location = New System.Drawing.Point(0, 0)
            Me.layoutChkReminder.Name = "layoutChkReminder"
            Me.layoutChkReminder.Size = New System.Drawing.Size(84, 24)
            Me.layoutChkReminder.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutChkReminder.TextToControlDistance = 0
            Me.layoutChkReminder.TextVisible = False
            '
            'layoutReminder
            '
            Me.layoutReminder.Control = Me.cbReminder
            resources.ApplyResources(Me.layoutReminder, "layoutReminder")
            Me.layoutReminder.Location = New System.Drawing.Point(84, 0)
            Me.layoutReminder.Name = "layoutReminder"
            Me.layoutReminder.Size = New System.Drawing.Size(505, 24)
            Me.layoutReminder.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutReminder.TextToControlDistance = 0
            Me.layoutReminder.TextVisible = False
            '
            'layoutStartGroup
            '
            resources.ApplyResources(Me.layoutStartGroup, "layoutStartGroup")
            Me.layoutStartGroup.GroupBordersVisible = False
            Me.layoutStartGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.layoutStartDate, Me.layoutStartTime})
            Me.layoutStartGroup.Location = New System.Drawing.Point(0, 147)
            Me.layoutStartGroup.Name = "layoutStartGroup"
            Me.layoutStartGroup.Size = New System.Drawing.Size(339, 48)
            '
            'layoutStartDate
            '
            Me.layoutStartDate.Control = Me.edtStartDate
            resources.ApplyResources(Me.layoutStartDate, "layoutStartDate")
            Me.layoutStartDate.Location = New System.Drawing.Point(0, 0)
            Me.layoutStartDate.Name = "layoutStartDate"
            Me.layoutStartDate.Size = New System.Drawing.Size(230, 48)
            Me.layoutStartDate.TextSize = New System.Drawing.Size(88, 13)
            '
            'layoutStartTime
            '
            Me.layoutStartTime.Control = Me.edtStartTime
            resources.ApplyResources(Me.layoutStartTime, "layoutStartTime")
            Me.layoutStartTime.Location = New System.Drawing.Point(230, 0)
            Me.layoutStartTime.Name = "layoutStartTime"
            Me.layoutStartTime.Size = New System.Drawing.Size(109, 48)
            Me.layoutStartTime.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutStartTime.TextToControlDistance = 0
            Me.layoutStartTime.TextVisible = False
            '
            'layoutEndGroup
            '
            resources.ApplyResources(Me.layoutEndGroup, "layoutEndGroup")
            Me.layoutEndGroup.GroupBordersVisible = False
            Me.layoutEndGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.layoutEndDate, Me.layoutEndTime})
            Me.layoutEndGroup.Location = New System.Drawing.Point(0, 195)
            Me.layoutEndGroup.Name = "layoutEndGroup"
            Me.layoutEndGroup.Size = New System.Drawing.Size(339, 24)
            '
            'layoutEndDate
            '
            Me.layoutEndDate.Control = Me.edtEndDate
            resources.ApplyResources(Me.layoutEndDate, "layoutEndDate")
            Me.layoutEndDate.Location = New System.Drawing.Point(0, 0)
            Me.layoutEndDate.Name = "layoutEndDate"
            Me.layoutEndDate.Size = New System.Drawing.Size(230, 24)
            Me.layoutEndDate.TextSize = New System.Drawing.Size(88, 13)
            '
            'layoutEndTime
            '
            Me.layoutEndTime.Control = Me.edtEndTime
            resources.ApplyResources(Me.layoutEndTime, "layoutEndTime")
            Me.layoutEndTime.Location = New System.Drawing.Point(230, 0)
            Me.layoutEndTime.Name = "layoutEndTime"
            Me.layoutEndTime.Size = New System.Drawing.Size(109, 24)
            Me.layoutEndTime.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutEndTime.TextToControlDistance = 0
            Me.layoutEndTime.TextVisible = False
            '
            'layoutResourcesGroup
            '
            resources.ApplyResources(Me.layoutResourcesGroup, "layoutResourcesGroup")
            Me.layoutResourcesGroup.GroupBordersVisible = False
            Me.layoutResourcesGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.layoutResources, Me.layoutResource})
            Me.layoutResourcesGroup.Location = New System.Drawing.Point(339, 147)
            Me.layoutResourcesGroup.Name = "layoutResourcesGroup"
            Me.layoutResourcesGroup.Size = New System.Drawing.Size(250, 48)
            '
            'layoutResources
            '
            Me.layoutResources.Control = Me.edtResources
            resources.ApplyResources(Me.layoutResources, "layoutResources")
            Me.layoutResources.Location = New System.Drawing.Point(0, 0)
            Me.layoutResources.Name = "layoutResources"
            Me.layoutResources.Size = New System.Drawing.Size(250, 24)
            Me.layoutResources.TextSize = New System.Drawing.Size(88, 13)
            '
            'layoutResource
            '
            Me.layoutResource.Control = Me.edtResource
            resources.ApplyResources(Me.layoutResource, "layoutResource")
            Me.layoutResource.Location = New System.Drawing.Point(0, 24)
            Me.layoutResource.Name = "layoutResource"
            Me.layoutResource.Size = New System.Drawing.Size(250, 24)
            Me.layoutResource.TextSize = New System.Drawing.Size(88, 13)
            '
            'layoutHorzSeparator1
            '
            Me.layoutHorzSeparator1.Control = Me.lblHorzSeparator1
            resources.ApplyResources(Me.layoutHorzSeparator1, "layoutHorzSeparator1")
            Me.layoutHorzSeparator1.Location = New System.Drawing.Point(0, 130)
            Me.layoutHorzSeparator1.Name = "layoutHorzSeparator1"
            Me.layoutHorzSeparator1.Size = New System.Drawing.Size(589, 17)
            Me.layoutHorzSeparator1.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutHorzSeparator1.TextToControlDistance = 0
            Me.layoutHorzSeparator1.TextVisible = False
            '
            'layoutHorzSeparator2
            '
            Me.layoutHorzSeparator2.Control = Me.lblHorzSeparator2
            resources.ApplyResources(Me.layoutHorzSeparator2, "layoutHorzSeparator2")
            Me.layoutHorzSeparator2.Location = New System.Drawing.Point(0, 219)
            Me.layoutHorzSeparator2.Name = "layoutHorzSeparator2"
            Me.layoutHorzSeparator2.Size = New System.Drawing.Size(589, 17)
            Me.layoutHorzSeparator2.TextSize = New System.Drawing.Size(0, 0)
            Me.layoutHorzSeparator2.TextToControlDistance = 0
            Me.layoutHorzSeparator2.TextVisible = False
            '
            'emptySpaceItem1
            '
            Me.emptySpaceItem1.AllowHotTrack = False
            resources.ApplyResources(Me.emptySpaceItem1, "emptySpaceItem1")
            Me.emptySpaceItem1.Location = New System.Drawing.Point(441, 195)
            Me.emptySpaceItem1.Name = "emptySpaceItem1"
            Me.emptySpaceItem1.Size = New System.Drawing.Size(148, 24)
            Me.emptySpaceItem1.TextSize = New System.Drawing.Size(0, 0)
            '
            'LayoutControlItem3
            '
            Me.LayoutControlItem3.Control = Me.SearchLookUpEdit1
            resources.ApplyResources(Me.LayoutControlItem3, "LayoutControlItem3")
            Me.LayoutControlItem3.Location = New System.Drawing.Point(0, 58)
            Me.LayoutControlItem3.Name = "LayoutControlItem3"
            Me.LayoutControlItem3.Size = New System.Drawing.Size(589, 24)
            Me.LayoutControlItem3.TextSize = New System.Drawing.Size(88, 13)
            '
            'LayoutControlItem1
            '
            Me.LayoutControlItem1.Control = Me.edtResources
            resources.ApplyResources(Me.LayoutControlItem1, "LayoutControlItem1")
            Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlItem1.Name = "layoutResources"
            Me.LayoutControlItem1.Size = New System.Drawing.Size(250, 24)
            Me.LayoutControlItem1.TextSize = New System.Drawing.Size(51, 13)
            Me.LayoutControlItem1.TextToControlDistance = 5
            '
            'BarEditItem2
            '
            Me.BarEditItem2.Edit = Nothing
            Me.BarEditItem2.Id = 57
            Me.BarEditItem2.Name = "BarEditItem2"
            resources.ApplyResources(Me.BarEditItem2, "BarEditItem2")
            '
            'BarEditItem1
            '
            Me.BarEditItem1.Edit = Nothing
            Me.BarEditItem1.Id = 57
            Me.BarEditItem1.Name = "BarEditItem1"
            resources.ApplyResources(Me.BarEditItem1, "BarEditItem1")
            '
            'BarEditItem3
            '
            Me.BarEditItem3.Edit = Nothing
            Me.BarEditItem3.Id = 57
            Me.BarEditItem3.Name = "BarEditItem3"
            resources.ApplyResources(Me.BarEditItem3, "BarEditItem3")
            '
            'AppointmentFormOutlook2007Style
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.layoutCtrl)
            Me.Controls.Add(Me.ribbonControl)
            Me.Name = "AppointmentFormOutlook2007Style"
            Me.Ribbon = Me.ribbonControl
            Me.ShowInTaskbar = False
            CType(Me.repItemAppointmentStatus, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.repItemAppointmentLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ribbonControl, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.applicationMenu, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.barAndDockingController, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.tbLocation.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutCtrl, System.ComponentModel.ISupportInitialize).EndInit()
            Me.layoutCtrl.ResumeLayout(False)
            CType(Me.SearchLookUpEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.SearchLookUpEdit1View, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtResources.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtResources.ResourcesCheckedListBoxControl, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.tbSubject.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtStartDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtStartDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.cbReminder.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtResource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtEndDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtEndDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.tbDescription.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtStartTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.edtEndTime.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkAllDay.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkReminder.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutControlGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutLocation, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutSubject, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutAllDay, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutInfo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutReminderGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutChkReminder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutReminder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutStartGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutStartDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutStartTime, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutEndGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutEndDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutEndTime, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutResourcesGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutResources, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutResource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutHorzSeparator1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.layoutHorzSeparator2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.emptySpaceItem1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

		#End Region

		Protected Friend ribbonControl As DevExpress.XtraBars.Ribbon.RibbonControl
		Protected Friend WithEvents btnSave As DevExpress.XtraBars.BarButtonItem
		Protected Friend WithEvents btnDelete As DevExpress.XtraBars.BarButtonItem
		Protected Friend WithEvents btnRecurrence As DevExpress.XtraBars.BarButtonItem
		Protected Friend WithEvents btnSpelling As DevExpress.XtraBars.BarButtonItem
		Protected Friend WithEvents btnClose As DevExpress.XtraBars.BarButtonItem
		Protected Friend WithEvents btnSaveAndClose As DevExpress.XtraBars.BarButtonItem
		Protected Friend ribPageAppointment As DevExpress.XtraBars.Ribbon.RibbonPage
		Protected Friend pgrActions As DevExpress.XtraBars.Ribbon.RibbonPageGroup
		Protected Friend pgrOptions As DevExpress.XtraBars.Ribbon.RibbonPageGroup
		Protected Friend pgrProofing As DevExpress.XtraBars.Ribbon.RibbonPageGroup
		Protected Friend spellChecker As DevExpress.XtraSpellChecker.SpellChecker
		Protected Friend applicationMenu As DevExpress.XtraBars.Ribbon.ApplicationMenu
		Protected Friend barAndDockingController As DevExpress.XtraBars.BarAndDockingController
		Protected chkReminder As DevExpress.XtraEditors.CheckEdit
		Protected chkAllDay As DevExpress.XtraEditors.CheckEdit
		Protected edtEndDate As DevExpress.XtraEditors.DateEdit
		Protected edtStartDate As DevExpress.XtraEditors.DateEdit
		Protected edtStartTime As DevExpress.XtraScheduler.UI.SchedulerTimeEdit
		Protected edtEndTime As DevExpress.XtraScheduler.UI.SchedulerTimeEdit
		Protected tbSubject As DevExpress.XtraEditors.TextEdit
		Protected tbLocation As DevExpress.XtraEditors.TextEdit
		Protected edtResources As DevExpress.XtraScheduler.UI.AppointmentResourcesEdit
		Protected edtResource As DevExpress.XtraScheduler.UI.AppointmentResourceEdit
		Protected tbDescription As DevExpress.XtraEditors.MemoEdit
		Protected cbReminder As DevExpress.XtraScheduler.UI.DurationEdit
		Protected lblInfo As DevExpress.XtraEditors.LabelControl
		Protected layoutCtrl As DevExpress.XtraLayout.LayoutControl
		Protected layoutControlGroup As DevExpress.XtraLayout.LayoutControlGroup
		Protected layoutResources As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutLocation As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutSubject As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutEndDate As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutStartDate As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutReminder As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutDescription As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutStartTime As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutEndTime As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutChkReminder As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutAllDay As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutInfo As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutResource As DevExpress.XtraLayout.LayoutControlItem
		Protected layoutReminderGroup As DevExpress.XtraLayout.LayoutControlGroup
		Protected layoutStartGroup As DevExpress.XtraLayout.LayoutControlGroup
		Protected layoutEndGroup As DevExpress.XtraLayout.LayoutControlGroup
		Protected layoutResourcesGroup As DevExpress.XtraLayout.LayoutControlGroup
		Protected lblHorzSeparator1 As DevExpress.XtraEditors.LabelControl
		Protected layoutHorzSeparator1 As DevExpress.XtraLayout.LayoutControlItem
		Protected lblHorzSeparator2 As DevExpress.XtraEditors.LabelControl
		Protected layoutHorzSeparator2 As DevExpress.XtraLayout.LayoutControlItem
		Protected emptySpaceItem1 As DevExpress.XtraLayout.EmptySpaceItem
		Protected repItemAppointmentStatus As RepositoryItemAppointmentStatus
		Protected repItemAppointmentLabel As RepositoryItemAppointmentLabel
		Protected barEditShowTimeAs As DevExpress.XtraBars.BarEditItem
        Protected barEditLabelAs As DevExpress.XtraBars.BarEditItem
        Protected WithEvents LayoutControlItem1 As DevExpress.XtraLayout.LayoutControlItem
        Friend WithEvents BarEditItem2 As DevExpress.XtraBars.BarEditItem
        Friend WithEvents BarEditItem1 As DevExpress.XtraBars.BarEditItem
        Friend WithEvents BarEditItem3 As DevExpress.XtraBars.BarEditItem
        Friend WithEvents SearchLookUpEdit1 As DevExpress.XtraEditors.SearchLookUpEdit
        Friend WithEvents SearchLookUpEdit1View As DevExpress.XtraGrid.Views.Grid.GridView
        Friend WithEvents LayoutControlItem3 As DevExpress.XtraLayout.LayoutControlItem
	End Class
End Namespace