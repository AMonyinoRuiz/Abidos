#Region "Copyright Syncfusion Inc. 2001 - 2011"
'
'  Copyright Syncfusion Inc. 2001 - 2011. All rights reserved.
'
'  Use of this code is subject to the terms of our license.
'  A copy of the current license can be obtained at any time by e-mailing
'  licensing@syncfusion.com. Any infringement will be prosecuted under
'  applicable laws. 
'
#End Region

Imports Infragistics.Win.UltraWinGrid
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection
Imports System.Diagnostics
Imports Syncfusion.Windows.Forms.Diagram.Controls
'Imports Syncfusion.Scripting
'Imports Syncfusion.Scripting.Design
Imports Syncfusion.Windows.Forms.Diagram
'Imports Syncfusion.Diagram.Windows.Text
Imports Syncfusion.Windows.Forms.Tools.XPMenus
Imports Syncfusion.SVG.IO
Imports System.Drawing.Imaging
Imports System.Drawing.Printing

'Namespace Syncfusion.Windows.Forms.Diagram.Samples.DiagramTool
''' <summary>
''' Summary description for DiagramForm.
''' </summary>
Public Class frmPlano : Inherits M_GenericForm.frmBase
    Implements IDisposable

    Dim oLinqPropuestaPlano As Propuesta_Plano
    Dim oDTC As DTCDataContext
    Dim oTipusObjectes As TipusObjectes
    Dim _PaletaGroupView2 As New Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView

    Enum TipusObjectes
        Rectangle = 1
        RondedRectangle = 2
        Square = 3
        ShadowedBox = 4
        Circle = 5
        Elipse = 6
        Triangle = 7
        RightTriangle = 8
        Pentagon = 9
        Hexagon = 10
        Octagon = 11
        Heptagon = 12
        RoundedSquare = 13
        Star6 = 14
    End Enum


#Region "Form controls"
    Private document As Model
    Public childFrameBarManager As ChildFrameBarManager
    Private smBarItemImages As ImageList
    Public WithEvents barItemSelect As BarItem
    Public WithEvents barItemLine As BarItem
    Public WithEvents barItemRectangle As BarItem
    Public WithEvents barItemEllipse As BarItem
    Public WithEvents barItemText As BarItem
    Public WithEvents barItemPolyline As BarItem
    Public WithEvents barItemPolygon As BarItem
    Public WithEvents barItemSpline As BarItem
    Public WithEvents barItemCurve As BarItem
    Public WithEvents barItemClosedCurve As BarItem
    Public WithEvents barItemImage As BarItem
    Public WithEvents barItemGroup As BarItem
    Public WithEvents barItemUngroup As BarItem
    Public WithEvents barItemBringToFront As BarItem
    Public WithEvents barItemSendToBack As BarItem
    Public WithEvents barItemBringForward As BarItem
    Public WithEvents barItemSendBackward As BarItem
    Public WithEvents barItemNudgeUp As BarItem
    Public WithEvents barItemNudgeDown As BarItem
    Public WithEvents barItemNudgeLeft As BarItem
    Public WithEvents barItemNudgeRight As BarItem
    Public WithEvents barItemRotateLeft As BarItem
    Public WithEvents barItemRotateRight As BarItem
    Public WithEvents barItemFlipVertical As BarItem
    Public WithEvents barItemFlipHorizontal As BarItem
    Public barDrawing As Bar
    Public barNode As Bar
    Public barNudge As Bar
    Public barRotate As Bar
    Public WithEvents barItemPan As BarItem
    Public bar1 As Bar
    Private components As IContainer
    Public WithEvents barItemShowGrid As BarItem
    Public WithEvents barItemSnapToGrid As BarItem
    Public WithEvents barItemZoom As BarItem
    Public WithEvents comboBoxBarItemMagnification As ComboBoxBarItem
    Public WithEvents barItemOrthogonalLink As BarItem
    Public WithEvents barItemLink As BarItem
    Public barLinks As Bar
    Public WithEvents barItemDirectedLink As BarItem
    Public WithEvents barItemRichText As BarItem
    Public WithEvents barItemRoundRect As BarItem
    Public WithEvents barItemPencil As BarItem
    Public WithEvents barItemBoldText As BarItem
    Public WithEvents barItemAlignTextLeft As BarItem
    Public WithEvents barItemCenterText As BarItem
    Public WithEvents barItemAlignTextRight As BarItem
    Public comboBoxBarItemFontFamily As ComboBoxBarItem
    Public comboBoxBarItemPointSize As ComboBoxBarItem
    Public bar2 As Bar
    Public WithEvents barItemItalicText As BarItem
    Public WithEvents barItemUnderlineText As BarItem
    Public WithEvents barItemTextColor As BarItem
    Public bar3 As Bar
    Public WithEvents barItemLoadScript As BarItem
    Public WithEvents barItemRunScript As BarItem
    Public WithEvents barItemEditScript As BarItem
    Public WithEvents barItemStopScript As BarItem
    Public WithEvents barItemSuperscript As BarItem
    Public WithEvents barItemSubscript As BarItem
    Public WithEvents barItemUpper As BarItem
    Public WithEvents barItemLower As BarItem
    Public fileName_Renamed As String
    Public WithEvents barItemStrikeoutText As BarItem
    Public WithEvents barItemBezier As BarItem
    Public barLayout As Bar
    Public barAlign As Bar
    Public WithEvents barItemSpaceAcross As BarItem
    Public WithEvents barItemSpaceDown As BarItem
    Public WithEvents barItemSameWidth As BarItem
    Public WithEvents barItemSameHeight As BarItem
    Public WithEvents barItemSameSize As BarItem
    Public WithEvents barItemAlignLeft As BarItem
    Public WithEvents barItemAlignCenter As BarItem
    Public WithEvents barItemAlignRight As BarItem
    Public WithEvents barItemAlignTop As BarItem
    Public WithEvents barItemAlignMiddle As BarItem
    Public WithEvents barItemAlignBottom As BarItem
    Public contextMenu1 As ContextMenu
    Public mnuAlgn As MenuItem
    Public mnuFlip As MenuItem
    Public WithEvents mnuAlgnLeft As MenuItem
    Public WithEvents mnuAlgnCenter As MenuItem
    Public WithEvents mnuAlgnRight As MenuItem
    Public WithEvents mnuAlgnTop As MenuItem
    Public WithEvents mnuAlgnMiddle As MenuItem
    Public WithEvents mnuAlgnBottom As MenuItem
    Public mnuGrouping As MenuItem
    Public mnuOrder As MenuItem
    Public mnuRotate As MenuItem
    Public mnuResize As MenuItem
    Public WithEvents mnuFlipHoriz As MenuItem
    Public WithEvents mnuFlipVert As MenuItem
    Public WithEvents mnuFlipBoth As MenuItem
    Public WithEvents mnuGGroup As MenuItem
    Public WithEvents mnuOrdBTF As MenuItem
    Public WithEvents mnuOrdBF As MenuItem
    Public WithEvents mnuOrdSB As MenuItem
    Public WithEvents mnuOrdSTB As MenuItem
    Public WithEvents mnuLayout As MenuItem
    Public WithEvents mnuRtClockwise As MenuItem
    Public WithEvents mnuRtCClockwise As MenuItem
    Public WithEvents mnuRsSameWidth As MenuItem
    Public WithEvents mnuRsSameHeight As MenuItem
    Public WithEvents mnuRsSameSize As MenuItem
    Public WithEvents mnuRsSpaseAcross As MenuItem
    Public WithEvents mnuRsSpaceDown As MenuItem
    Public WithEvents mnuGUngroup As System.Windows.Forms.MenuItem
    Public m_biSelectedAlignment As BarItem = Nothing
    Private superToolTip1 As Global.Syncfusion.Windows.Forms.Tools.SuperToolTip
    Private WithEvents openDiagramDialog As System.Windows.Forms.OpenFileDialog
    Private WithEvents saveDiagramDialog As System.Windows.Forms.SaveFileDialog
    Public WithEvents barGeneral As Syncfusion.Windows.Forms.Tools.XPMenus.Bar
    Friend WithEvents barGuardar As Syncfusion.Windows.Forms.Tools.XPMenus.BarItem
    Private WithEvents smallImageList As System.Windows.Forms.ImageList
    Friend WithEvents barGuardarComo As Syncfusion.Windows.Forms.Tools.XPMenus.BarItem
    Friend WithEvents barPageSetup As Syncfusion.Windows.Forms.Tools.XPMenus.BarItem
    Friend WithEvents barPrintPreview As Syncfusion.Windows.Forms.Tools.XPMenus.BarItem
    Friend WithEvents barImprimir As Syncfusion.Windows.Forms.Tools.XPMenus.BarItem
    Friend WithEvents GRD_Linea As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Familia As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraPanel1 As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents paletteGroupBar1 As Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar
    Private WithEvents diagramComponent As Syncfusion.Windows.Forms.Diagram.Controls.Diagram
    Private WithEvents propertyEditor1 As Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor
    Private WithEvents paletteGroupView1 As Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView
    Friend WithEvents GroupBarItem1 As Syncfusion.Windows.Forms.Tools.GroupBarItem
    Friend WithEvents L_NotaInformativa As Infragistics.Win.Misc.UltraLabel
    Private toolTipInfo As Global.Syncfusion.Windows.Forms.Tools.ToolTipInfo
#End Region

#Region "diagramForm members"

    Private Property CurrentAlignment() As BarItem
        Get
            Return m_biSelectedAlignment
        End Get
        Set(ByVal value As BarItem)
            If Not m_biSelectedAlignment Is value Then
                ' Uncheck current.
                If Not m_biSelectedAlignment Is Nothing Then
                    m_biSelectedAlignment.Checked = False
                End If

                ' Set new value
                m_biSelectedAlignment = value

                ' Check new.
                If Not m_biSelectedAlignment Is Nothing Then
                    m_biSelectedAlignment.Checked = True
                End If
            End If
        End Set
    End Property

    Public Sub New(ByVal mdiParent As Form)
        '
        ' Required for Windows Form Designer support
        '
        InitializeComponent()
        Me.MdiParent = mdiParent

        Me.diagramComponent.Model = Me.document

        '
        ' Wire up event handlers to canvas
        '
        AddHandler CType(diagramComponent.EventSink, DiagramViewerEventSink).ToolActivated, AddressOf DiagramForm_ToolActivated
        ' Setup the Diagram control for scripting capability
        ' Me.InitializeScriptingManager()

        ' Load up names of fonts that can be selected
        Me.LoadFontSelections()

        ' Feedback with TextFormatting toolbar
        AddHandler diagramComponent.Controller.TextEditor.FormatChanged, AddressOf FormatChanged
        AddHandler Application.Idle, AddressOf Application_Idle
    End Sub

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    'Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    '    If disposing Then
    '        If Not components Is Nothing Then
    '            components.Dispose()
    '        End If

    '        'AddHandler Me.scriptingMgr.CompileError, AddressOf Me.scriptingMgr_CompileError
    '    End If
    '    MyBase.Dispose(disposing)
    'End Sub

    Public ReadOnly Property Diagram() As Controls.Diagram
        Get
            Return Me.diagramComponent
        End Get
    End Property

    Public Sub OpenFile(ByVal strFileName As String)

        Me.diagramComponent.LoadBinary(strFileName)
        Me.FileName = strFileName

    End Sub

    Public Sub SaveFile()
        If (Not Me.HasFileName) Then
            Throw New InvalidOperationException()
        End If

        Me.SaveAsFile(Me.fileName_Renamed)
    End Sub

    Public Sub SaveAsFile(ByVal strFileName As String)
        Dim oStream As FileStream

        Try
            oStream = New FileStream(strFileName, FileMode.Create)
        Catch ex As Exception
            oStream = Nothing ' just to be sure
            MessageBox.Show("Error opening " & strFileName & " - " & ex.Message)
        End Try

        If Not oStream Is Nothing Then
            Try
                diagramComponent.SaveBinary(oStream)
                ' Save the DiagramScript object to the serialization stream along with the diagram document
                Dim formatter As BinaryFormatter = New BinaryFormatter()
                'formatter.Binder = Syncfusion.Runtime.Serialization.AppStateSerializer.CustomBinder
                'formatter.AssemblyFormat = FormatterAssemblyStyle.Simple
                'formatter.Serialize(oStream, Me.scriptingMgr.Script)

                Me.FileName = strFileName
            Catch ex As Exception
                MessageBox.Show("Serialization error - " & ex.Message)
            Finally
                oStream.Close()
            End Try
        End If
    End Sub

    Public Property FileName() As String
        Get
            Return Me.fileName_Renamed
        End Get
        Set(ByVal value As String)
            Me.fileName_Renamed = value
            Me.Text = Path.GetFileNameWithoutExtension(Me.fileName_Renamed)
        End Set
    End Property

    Public ReadOnly Property HasFileName() As Boolean
        Get
            Return (Not Me.fileName_Renamed Is Nothing AndAlso Me.fileName_Renamed.Length > 0)
        End Get
    End Property

    Protected ReadOnly Property PropertyEditor() As PropertyEditor
        Get
            If Not Me.MdiParent Is Nothing Then
                Dim mainForm As Principal = CType(IIf(TypeOf Me.MdiParent Is Principal, Me.MdiParent, Nothing), Principal)
                If Not mainForm Is Nothing Then
                    ' Return mainForm.PropertyEditor
                End If
            End If
            Return Nothing
        End Get
    End Property

#End Region

#Region "Windows Form Designer generated code"
    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPlano))
        Me.contextMenu1 = New System.Windows.Forms.ContextMenu()
        Me.mnuAlgn = New System.Windows.Forms.MenuItem()
        Me.mnuAlgnLeft = New System.Windows.Forms.MenuItem()
        Me.mnuAlgnCenter = New System.Windows.Forms.MenuItem()
        Me.mnuAlgnRight = New System.Windows.Forms.MenuItem()
        Me.mnuAlgnTop = New System.Windows.Forms.MenuItem()
        Me.mnuAlgnMiddle = New System.Windows.Forms.MenuItem()
        Me.mnuAlgnBottom = New System.Windows.Forms.MenuItem()
        Me.mnuFlip = New System.Windows.Forms.MenuItem()
        Me.mnuFlipHoriz = New System.Windows.Forms.MenuItem()
        Me.mnuFlipVert = New System.Windows.Forms.MenuItem()
        Me.mnuFlipBoth = New System.Windows.Forms.MenuItem()
        Me.mnuGrouping = New System.Windows.Forms.MenuItem()
        Me.mnuGGroup = New System.Windows.Forms.MenuItem()
        Me.mnuGUngroup = New System.Windows.Forms.MenuItem()
        Me.mnuOrder = New System.Windows.Forms.MenuItem()
        Me.mnuOrdBTF = New System.Windows.Forms.MenuItem()
        Me.mnuOrdBF = New System.Windows.Forms.MenuItem()
        Me.mnuOrdSB = New System.Windows.Forms.MenuItem()
        Me.mnuOrdSTB = New System.Windows.Forms.MenuItem()
        Me.mnuRotate = New System.Windows.Forms.MenuItem()
        Me.mnuRtClockwise = New System.Windows.Forms.MenuItem()
        Me.mnuRtCClockwise = New System.Windows.Forms.MenuItem()
        Me.mnuResize = New System.Windows.Forms.MenuItem()
        Me.mnuRsSameWidth = New System.Windows.Forms.MenuItem()
        Me.mnuRsSameHeight = New System.Windows.Forms.MenuItem()
        Me.mnuRsSameSize = New System.Windows.Forms.MenuItem()
        Me.mnuRsSpaseAcross = New System.Windows.Forms.MenuItem()
        Me.mnuRsSpaceDown = New System.Windows.Forms.MenuItem()
        Me.mnuLayout = New System.Windows.Forms.MenuItem()
        Me.document = New Syncfusion.Windows.Forms.Diagram.Model(Me.components)
        Me.childFrameBarManager = New Syncfusion.Windows.Forms.Tools.XPMenus.ChildFrameBarManager(Me)
        Me.barDrawing = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Drawing")
        Me.barItemSelect = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.smBarItemImages = New System.Windows.Forms.ImageList(Me.components)
        Me.barItemLine = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemPolyline = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemRectangle = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemRoundRect = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemPencil = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemEllipse = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemPolygon = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemCurve = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemClosedCurve = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSpline = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemBezier = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemRichText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemImage = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barNode = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Node")
        Me.barItemGroup = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemUngroup = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemBringToFront = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSendToBack = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemBringForward = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSendBackward = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barNudge = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Nudge")
        Me.barItemNudgeUp = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemNudgeDown = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemNudgeLeft = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemNudgeRight = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barLinks = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Links")
        Me.barItemLink = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemOrthogonalLink = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemDirectedLink = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barRotate = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Rotate")
        Me.barItemRotateLeft = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemRotateRight = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemFlipVertical = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemFlipHorizontal = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.bar1 = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "View")
        Me.barItemPan = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemShowGrid = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSnapToGrid = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemZoom = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.comboBoxBarItemMagnification = New Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem()
        Me.bar2 = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Text Formatting")
        Me.comboBoxBarItemFontFamily = New Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem()
        Me.comboBoxBarItemPointSize = New Syncfusion.Windows.Forms.Tools.XPMenus.ComboBoxBarItem()
        Me.barItemBoldText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemItalicText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemUnderlineText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemStrikeoutText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignTextLeft = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemCenterText = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignTextRight = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemTextColor = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSubscript = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSuperscript = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemLower = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemUpper = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.bar3 = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Scripting")
        Me.barItemLoadScript = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemRunScript = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemEditScript = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemStopScript = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barLayout = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Layout")
        Me.barItemSpaceAcross = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSpaceDown = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSameWidth = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSameHeight = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemSameSize = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barAlign = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "Align")
        Me.barItemAlignLeft = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignCenter = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignRight = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignTop = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignMiddle = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barItemAlignBottom = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barGeneral = New Syncfusion.Windows.Forms.Tools.XPMenus.Bar(Me.childFrameBarManager, "General")
        Me.barGuardar = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.smallImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.barGuardarComo = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barPageSetup = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barPrintPreview = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.barImprimir = New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem()
        Me.superToolTip1 = New Syncfusion.Windows.Forms.Tools.SuperToolTip(Me)
        Me.openDiagramDialog = New System.Windows.Forms.OpenFileDialog()
        Me.saveDiagramDialog = New System.Windows.Forms.SaveFileDialog()
        Me.GRD_Linea = New M_UltraGrid.m_UltraGrid()
        Me.C_Familia = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraPanel1 = New Infragistics.Win.Misc.UltraPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.diagramComponent = New Syncfusion.Windows.Forms.Diagram.Controls.Diagram(Me.components)
        Me.propertyEditor1 = New Syncfusion.Windows.Forms.Diagram.Controls.PropertyEditor(Me.components)
        Me.paletteGroupBar1 = New Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupBar(Me.components)
        Me.paletteGroupView1 = New Syncfusion.Windows.Forms.Diagram.Controls.PaletteGroupView(Me.components)
        Me.GroupBarItem1 = New Syncfusion.Windows.Forms.Tools.GroupBarItem()
        Me.L_NotaInformativa = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.document, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.childFrameBarManager, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.comboBoxBarItemMagnification, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.comboBoxBarItemFontFamily, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.comboBoxBarItemPointSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Familia, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraPanel1.ClientArea.SuspendLayout()
        Me.UltraPanel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.diagramComponent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.paletteGroupBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.paletteGroupBar1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ToolForm.Location = New System.Drawing.Point(156, 6)
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        Me.ToolForm.Size = New System.Drawing.Size(68, 24)
        '
        'contextMenu1
        '
        Me.contextMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAlgn, Me.mnuFlip, Me.mnuGrouping, Me.mnuOrder, Me.mnuRotate, Me.mnuResize, Me.mnuLayout})
        '
        'mnuAlgn
        '
        Me.mnuAlgn.Index = 0
        Me.mnuAlgn.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAlgnLeft, Me.mnuAlgnCenter, Me.mnuAlgnRight, Me.mnuAlgnTop, Me.mnuAlgnMiddle, Me.mnuAlgnBottom})
        Me.mnuAlgn.Text = "Align"
        '
        'mnuAlgnLeft
        '
        Me.mnuAlgnLeft.Index = 0
        Me.mnuAlgnLeft.Text = "Align Left"
        '
        'mnuAlgnCenter
        '
        Me.mnuAlgnCenter.Index = 1
        Me.mnuAlgnCenter.Text = "Align Center"
        '
        'mnuAlgnRight
        '
        Me.mnuAlgnRight.Index = 2
        Me.mnuAlgnRight.Text = "Align Right"
        '
        'mnuAlgnTop
        '
        Me.mnuAlgnTop.Index = 3
        Me.mnuAlgnTop.Text = "Align Top"
        '
        'mnuAlgnMiddle
        '
        Me.mnuAlgnMiddle.Index = 4
        Me.mnuAlgnMiddle.Text = "Align Middle"
        '
        'mnuAlgnBottom
        '
        Me.mnuAlgnBottom.Index = 5
        Me.mnuAlgnBottom.Text = "Align Bottom"
        '
        'mnuFlip
        '
        Me.mnuFlip.Index = 1
        Me.mnuFlip.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFlipHoriz, Me.mnuFlipVert, Me.mnuFlipBoth})
        Me.mnuFlip.Text = "Flip"
        '
        'mnuFlipHoriz
        '
        Me.mnuFlipHoriz.Index = 0
        Me.mnuFlipHoriz.Text = "Flip Horizontally"
        '
        'mnuFlipVert
        '
        Me.mnuFlipVert.Index = 1
        Me.mnuFlipVert.Text = "Flip Vertically"
        '
        'mnuFlipBoth
        '
        Me.mnuFlipBoth.Index = 2
        Me.mnuFlipBoth.Text = "Flip Both"
        '
        'mnuGrouping
        '
        Me.mnuGrouping.Index = 2
        Me.mnuGrouping.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuGGroup, Me.mnuGUngroup})
        Me.mnuGrouping.Text = "Grouping"
        '
        'mnuGGroup
        '
        Me.mnuGGroup.Index = 0
        Me.mnuGGroup.Text = "Group"
        '
        'mnuGUngroup
        '
        Me.mnuGUngroup.Index = 1
        Me.mnuGUngroup.Text = "Ungroup"
        '
        'mnuOrder
        '
        Me.mnuOrder.Index = 3
        Me.mnuOrder.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOrdBTF, Me.mnuOrdBF, Me.mnuOrdSB, Me.mnuOrdSTB})
        Me.mnuOrder.Text = "Order"
        '
        'mnuOrdBTF
        '
        Me.mnuOrdBTF.Index = 0
        Me.mnuOrdBTF.Text = "Bring To Front"
        '
        'mnuOrdBF
        '
        Me.mnuOrdBF.Index = 1
        Me.mnuOrdBF.Text = "Bring Forward"
        '
        'mnuOrdSB
        '
        Me.mnuOrdSB.Index = 2
        Me.mnuOrdSB.Text = "Send Backward"
        '
        'mnuOrdSTB
        '
        Me.mnuOrdSTB.Index = 3
        Me.mnuOrdSTB.Text = "Send To Back"
        '
        'mnuRotate
        '
        Me.mnuRotate.Index = 4
        Me.mnuRotate.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuRtClockwise, Me.mnuRtCClockwise})
        Me.mnuRotate.Text = "Rotate"
        '
        'mnuRtClockwise
        '
        Me.mnuRtClockwise.Index = 0
        Me.mnuRtClockwise.Text = "Rotate 90 clockwise"
        '
        'mnuRtCClockwise
        '
        Me.mnuRtCClockwise.Index = 1
        Me.mnuRtCClockwise.Text = "Rotate 90 counter-clockwise"
        '
        'mnuResize
        '
        Me.mnuResize.Index = 5
        Me.mnuResize.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuRsSameWidth, Me.mnuRsSameHeight, Me.mnuRsSameSize, Me.mnuRsSpaseAcross, Me.mnuRsSpaceDown})
        Me.mnuResize.Text = "Resize"
        '
        'mnuRsSameWidth
        '
        Me.mnuRsSameWidth.Index = 0
        Me.mnuRsSameWidth.Text = "Same Width"
        '
        'mnuRsSameHeight
        '
        Me.mnuRsSameHeight.Index = 1
        Me.mnuRsSameHeight.Text = "Same Height"
        '
        'mnuRsSameSize
        '
        Me.mnuRsSameSize.Index = 2
        Me.mnuRsSameSize.Text = "Same Size"
        '
        'mnuRsSpaseAcross
        '
        Me.mnuRsSpaseAcross.Index = 3
        Me.mnuRsSpaseAcross.Text = "Space Across"
        '
        'mnuRsSpaceDown
        '
        Me.mnuRsSpaceDown.Index = 4
        Me.mnuRsSpaceDown.Text = "Space Down"
        '
        'mnuLayout
        '
        Me.mnuLayout.Index = 6
        Me.mnuLayout.Text = "Lay out nodes"
        '
        'document
        '
        Me.document.BackgroundStyle.PathBrushStyle = Syncfusion.Windows.Forms.Diagram.PathGradientBrushStyle.RectangleCenter
        Me.document.DocumentScale.DisplayName = "No Scale"
        Me.document.DocumentScale.Height = 1.0!
        Me.document.DocumentScale.Width = 1.0!
        Me.document.DocumentSize.Height = 1169.0!
        Me.document.DocumentSize.Width = 827.0!
        Me.document.LineStyle.DashPattern = Nothing
        Me.document.LineStyle.LineColor = System.Drawing.Color.Black
        Me.document.LogicalSize = New System.Drawing.SizeF(827.0!, 1169.0!)
        Me.document.ShadowStyle.Color = System.Drawing.Color.DimGray
        Me.document.ShadowStyle.ColorAlphaFactor = 255
        Me.document.ShadowStyle.ForeColor = System.Drawing.Color.DimGray
        Me.document.ShadowStyle.ForeColorAlphaFactor = 255
        '
        'childFrameBarManager
        '
        Me.childFrameBarManager.BarPositionInfo = CType(resources.GetObject("childFrameBarManager.BarPositionInfo"), System.IO.MemoryStream)
        Me.childFrameBarManager.Bars.Add(Me.barDrawing)
        Me.childFrameBarManager.Bars.Add(Me.barNode)
        Me.childFrameBarManager.Bars.Add(Me.barNudge)
        Me.childFrameBarManager.Bars.Add(Me.barLinks)
        Me.childFrameBarManager.Bars.Add(Me.barRotate)
        Me.childFrameBarManager.Bars.Add(Me.bar1)
        Me.childFrameBarManager.Bars.Add(Me.bar2)
        Me.childFrameBarManager.Bars.Add(Me.bar3)
        Me.childFrameBarManager.Bars.Add(Me.barLayout)
        Me.childFrameBarManager.Bars.Add(Me.barAlign)
        Me.childFrameBarManager.Bars.Add(Me.barGeneral)
        Me.childFrameBarManager.Categories.Add("General")
        Me.childFrameBarManager.Categories.Add("Drawing Tools")
        Me.childFrameBarManager.Categories.Add("Node Tools")
        Me.childFrameBarManager.Categories.Add("Connection Tools")
        Me.childFrameBarManager.Categories.Add("Nudge Tools")
        Me.childFrameBarManager.Categories.Add("Rotate Tools")
        Me.childFrameBarManager.Categories.Add("View Tools")
        Me.childFrameBarManager.Categories.Add("Text Formatting")
        Me.childFrameBarManager.Categories.Add("Scripting")
        Me.childFrameBarManager.Categories.Add("Align")
        Me.childFrameBarManager.Categories.Add("Layout")
        Me.childFrameBarManager.CurrentBaseFormType = "M_GenericForm.frmBase"
        Me.childFrameBarManager.Form = Me
        Me.childFrameBarManager.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barGuardar, Me.barGuardarComo, Me.barItemSelect, Me.barPageSetup, Me.barPrintPreview, Me.barImprimir, Me.barItemLine, Me.barItemRectangle, Me.barItemEllipse, Me.barItemText, Me.barItemPolyline, Me.barItemPolygon, Me.barItemSpline, Me.barItemBezier, Me.barItemGroup, Me.barItemCurve, Me.barItemClosedCurve, Me.barItemImage, Me.barItemRichText, Me.barItemUngroup, Me.barItemRoundRect, Me.barItemBringToFront, Me.barItemSendToBack, Me.barItemPencil, Me.barItemBringForward, Me.barItemSendBackward, Me.barItemLink, Me.barItemNudgeUp, Me.barItemNudgeDown, Me.barItemNudgeLeft, Me.barItemNudgeRight, Me.barItemRotateLeft, Me.barItemRotateRight, Me.barItemFlipVertical, Me.barItemFlipHorizontal, Me.barItemPan, Me.barItemShowGrid, Me.barItemSnapToGrid, Me.barItemZoom, Me.comboBoxBarItemMagnification, Me.barItemOrthogonalLink, Me.barItemDirectedLink, Me.barItemBoldText, Me.barItemItalicText, Me.barItemUnderlineText, Me.barItemAlignTextLeft, Me.barItemCenterText, Me.barItemAlignTextRight, Me.comboBoxBarItemFontFamily, Me.comboBoxBarItemPointSize, Me.barItemTextColor, Me.barItemLoadScript, Me.barItemRunScript, Me.barItemEditScript, Me.barItemStopScript, Me.barItemSuperscript, Me.barItemSubscript, Me.barItemUpper, Me.barItemLower, Me.barItemStrikeoutText, Me.barItemSpaceAcross, Me.barItemSpaceDown, Me.barItemSameWidth, Me.barItemSameHeight, Me.barItemSameSize, Me.barItemAlignLeft, Me.barItemAlignCenter, Me.barItemAlignRight, Me.barItemAlignTop, Me.barItemAlignMiddle, Me.barItemAlignBottom})
        Me.childFrameBarManager.Style = Syncfusion.Windows.Forms.VisualStyle.Office2003
        '
        'barDrawing
        '
        Me.barDrawing.BarName = "Drawing"
        Me.barDrawing.Caption = "Drawing"
        Me.barDrawing.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemSelect, Me.barItemLine, Me.barItemPolyline, Me.barItemRectangle, Me.barItemRoundRect, Me.barItemPencil, Me.barItemEllipse, Me.barItemPolygon, Me.barItemCurve, Me.barItemClosedCurve, Me.barItemSpline, Me.barItemBezier, Me.barItemText, Me.barItemRichText, Me.barItemImage})
        Me.barDrawing.Manager = Me.childFrameBarManager
        '
        'barItemSelect
        '
        Me.barItemSelect.CategoryIndex = 1
        Me.barItemSelect.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSelect.ID = "Pointer"
        Me.barItemSelect.ImageIndex = 0
        Me.barItemSelect.ImageList = Me.smBarItemImages
        Me.barItemSelect.ShowToolTipInPopUp = False
        Me.barItemSelect.Tag = "SelectTool"
        Me.barItemSelect.Text = "Select"
        Me.barItemSelect.Tooltip = "Select"
        '
        'smBarItemImages
        '
        Me.smBarItemImages.ImageStream = CType(resources.GetObject("smBarItemImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.smBarItemImages.TransparentColor = System.Drawing.Color.Fuchsia
        Me.smBarItemImages.Images.SetKeyName(0, "")
        Me.smBarItemImages.Images.SetKeyName(1, "")
        Me.smBarItemImages.Images.SetKeyName(2, "")
        Me.smBarItemImages.Images.SetKeyName(3, "")
        Me.smBarItemImages.Images.SetKeyName(4, "")
        Me.smBarItemImages.Images.SetKeyName(5, "")
        Me.smBarItemImages.Images.SetKeyName(6, "")
        Me.smBarItemImages.Images.SetKeyName(7, "")
        Me.smBarItemImages.Images.SetKeyName(8, "")
        Me.smBarItemImages.Images.SetKeyName(9, "")
        Me.smBarItemImages.Images.SetKeyName(10, "")
        Me.smBarItemImages.Images.SetKeyName(11, "")
        Me.smBarItemImages.Images.SetKeyName(12, "")
        Me.smBarItemImages.Images.SetKeyName(13, "")
        Me.smBarItemImages.Images.SetKeyName(14, "")
        Me.smBarItemImages.Images.SetKeyName(15, "")
        Me.smBarItemImages.Images.SetKeyName(16, "")
        Me.smBarItemImages.Images.SetKeyName(17, "")
        Me.smBarItemImages.Images.SetKeyName(18, "")
        Me.smBarItemImages.Images.SetKeyName(19, "")
        Me.smBarItemImages.Images.SetKeyName(20, "")
        Me.smBarItemImages.Images.SetKeyName(21, "")
        Me.smBarItemImages.Images.SetKeyName(22, "")
        Me.smBarItemImages.Images.SetKeyName(23, "")
        Me.smBarItemImages.Images.SetKeyName(24, "")
        Me.smBarItemImages.Images.SetKeyName(25, "")
        Me.smBarItemImages.Images.SetKeyName(26, "")
        Me.smBarItemImages.Images.SetKeyName(27, "")
        Me.smBarItemImages.Images.SetKeyName(28, "")
        Me.smBarItemImages.Images.SetKeyName(29, "")
        Me.smBarItemImages.Images.SetKeyName(30, "")
        Me.smBarItemImages.Images.SetKeyName(31, "")
        Me.smBarItemImages.Images.SetKeyName(32, "")
        Me.smBarItemImages.Images.SetKeyName(33, "")
        Me.smBarItemImages.Images.SetKeyName(34, "")
        Me.smBarItemImages.Images.SetKeyName(35, "")
        Me.smBarItemImages.Images.SetKeyName(36, "")
        Me.smBarItemImages.Images.SetKeyName(37, "")
        Me.smBarItemImages.Images.SetKeyName(38, "")
        Me.smBarItemImages.Images.SetKeyName(39, "")
        Me.smBarItemImages.Images.SetKeyName(40, "")
        Me.smBarItemImages.Images.SetKeyName(41, "")
        Me.smBarItemImages.Images.SetKeyName(42, "")
        Me.smBarItemImages.Images.SetKeyName(43, "")
        Me.smBarItemImages.Images.SetKeyName(44, "")
        Me.smBarItemImages.Images.SetKeyName(45, "")
        Me.smBarItemImages.Images.SetKeyName(46, "")
        Me.smBarItemImages.Images.SetKeyName(47, "")
        Me.smBarItemImages.Images.SetKeyName(48, "")
        Me.smBarItemImages.Images.SetKeyName(49, "")
        Me.smBarItemImages.Images.SetKeyName(50, "")
        Me.smBarItemImages.Images.SetKeyName(51, "")
        Me.smBarItemImages.Images.SetKeyName(52, "")
        Me.smBarItemImages.Images.SetKeyName(53, "")
        Me.smBarItemImages.Images.SetKeyName(54, "")
        Me.smBarItemImages.Images.SetKeyName(55, "")
        Me.smBarItemImages.Images.SetKeyName(56, "")
        Me.smBarItemImages.Images.SetKeyName(57, "")
        Me.smBarItemImages.Images.SetKeyName(58, "")
        Me.smBarItemImages.Images.SetKeyName(59, "")
        Me.smBarItemImages.Images.SetKeyName(60, "")
        Me.smBarItemImages.Images.SetKeyName(61, "")
        Me.smBarItemImages.Images.SetKeyName(62, "")
        Me.smBarItemImages.Images.SetKeyName(63, "")
        Me.smBarItemImages.Images.SetKeyName(64, "")
        Me.smBarItemImages.Images.SetKeyName(65, "")
        Me.smBarItemImages.Images.SetKeyName(66, "")
        Me.smBarItemImages.Images.SetKeyName(67, "")
        Me.smBarItemImages.Images.SetKeyName(68, "")
        Me.smBarItemImages.Images.SetKeyName(69, "")
        Me.smBarItemImages.Images.SetKeyName(70, "")
        Me.smBarItemImages.Images.SetKeyName(71, "")
        Me.smBarItemImages.Images.SetKeyName(72, "")
        Me.smBarItemImages.Images.SetKeyName(73, "")
        Me.smBarItemImages.Images.SetKeyName(74, "")
        Me.smBarItemImages.Images.SetKeyName(75, "")
        Me.smBarItemImages.Images.SetKeyName(76, "pencil.png")
        '
        'barItemLine
        '
        Me.barItemLine.CategoryIndex = 1
        Me.barItemLine.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemLine.ID = "Line"
        Me.barItemLine.ImageIndex = 1
        Me.barItemLine.ImageList = Me.smBarItemImages
        Me.barItemLine.ShowToolTipInPopUp = False
        Me.barItemLine.Tag = "LineTool"
        Me.barItemLine.Text = "Line"
        Me.barItemLine.Tooltip = "Line"
        '
        'barItemPolyline
        '
        Me.barItemPolyline.CategoryIndex = 1
        Me.barItemPolyline.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemPolyline.ID = "Polyline"
        Me.barItemPolyline.ImageIndex = 2
        Me.barItemPolyline.ImageList = Me.smBarItemImages
        Me.barItemPolyline.ShowToolTipInPopUp = False
        Me.barItemPolyline.Tag = "PolyLineTool"
        Me.barItemPolyline.Text = "Polyline"
        Me.barItemPolyline.Tooltip = "Polyline"
        '
        'barItemRectangle
        '
        Me.barItemRectangle.CategoryIndex = 1
        Me.barItemRectangle.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemRectangle.ID = "Rectangle"
        Me.barItemRectangle.ImageIndex = 4
        Me.barItemRectangle.ImageList = Me.smBarItemImages
        Me.barItemRectangle.ShowToolTipInPopUp = False
        Me.barItemRectangle.Tag = "RectangleTool"
        Me.barItemRectangle.Text = "Rectangle"
        Me.barItemRectangle.Tooltip = "Rectangle"
        '
        'barItemRoundRect
        '
        Me.barItemRoundRect.CategoryIndex = 1
        Me.barItemRoundRect.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemRoundRect.ID = "Rounded Rectangle"
        Me.barItemRoundRect.ImageIndex = 5
        Me.barItemRoundRect.ImageList = Me.smBarItemImages
        Me.barItemRoundRect.ShowToolTipInPopUp = False
        Me.barItemRoundRect.Tag = "RoundRectTool"
        Me.barItemRoundRect.Text = "Rounded Rectangle"
        '
        'barItemPencil
        '
        Me.barItemPencil.CategoryIndex = 1
        Me.barItemPencil.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemPencil.ID = "Pencil Tool"
        Me.barItemPencil.ImageIndex = 76
        Me.barItemPencil.ImageList = Me.smBarItemImages
        Me.barItemPencil.ShowToolTipInPopUp = False
        Me.barItemPencil.Tag = "PencilTool"
        Me.barItemPencil.Text = "PencilTool"
        '
        'barItemEllipse
        '
        Me.barItemEllipse.CategoryIndex = 1
        Me.barItemEllipse.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemEllipse.ID = "Ellipse"
        Me.barItemEllipse.ImageIndex = 7
        Me.barItemEllipse.ImageList = Me.smBarItemImages
        Me.barItemEllipse.ShowToolTipInPopUp = False
        Me.barItemEllipse.Tag = "EllipseTool"
        Me.barItemEllipse.Text = "Ellipse"
        Me.barItemEllipse.Tooltip = "Ellipse"
        '
        'barItemPolygon
        '
        Me.barItemPolygon.CategoryIndex = 1
        Me.barItemPolygon.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemPolygon.ID = "Polygon"
        Me.barItemPolygon.ImageIndex = 6
        Me.barItemPolygon.ImageList = Me.smBarItemImages
        Me.barItemPolygon.ShowToolTipInPopUp = False
        Me.barItemPolygon.Tag = "PolygonTool"
        Me.barItemPolygon.Text = "Polygon"
        Me.barItemPolygon.Tooltip = "Polygon"
        '
        'barItemCurve
        '
        Me.barItemCurve.CategoryIndex = 1
        Me.barItemCurve.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemCurve.ID = "Curve"
        Me.barItemCurve.ImageIndex = 9
        Me.barItemCurve.ImageList = Me.smBarItemImages
        Me.barItemCurve.ShowToolTipInPopUp = False
        Me.barItemCurve.Tag = "CurveTool"
        Me.barItemCurve.Text = "Curve"
        Me.barItemCurve.Tooltip = "Curve"
        '
        'barItemClosedCurve
        '
        Me.barItemClosedCurve.CategoryIndex = 1
        Me.barItemClosedCurve.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemClosedCurve.ID = "Closed Curve"
        Me.barItemClosedCurve.ImageIndex = 10
        Me.barItemClosedCurve.ImageList = Me.smBarItemImages
        Me.barItemClosedCurve.ShowToolTipInPopUp = False
        Me.barItemClosedCurve.Tag = "ClosedCurveTool"
        Me.barItemClosedCurve.Text = "Closed Curve"
        Me.barItemClosedCurve.Tooltip = "Closed Curve"
        '
        'barItemSpline
        '
        Me.barItemSpline.CategoryIndex = 1
        Me.barItemSpline.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSpline.ID = "Spline"
        Me.barItemSpline.ImageIndex = 8
        Me.barItemSpline.ImageList = Me.smBarItemImages
        Me.barItemSpline.ShowToolTipInPopUp = False
        Me.barItemSpline.Tag = "SplineTool"
        Me.barItemSpline.Text = "Spline"
        Me.barItemSpline.Tooltip = "Spline"
        '
        'barItemBezier
        '
        Me.barItemBezier.CategoryIndex = 1
        Me.barItemBezier.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemBezier.ID = "Bezier"
        Me.barItemBezier.ImageIndex = 64
        Me.barItemBezier.ImageList = Me.smBarItemImages
        Me.barItemBezier.ShowToolTipInPopUp = False
        Me.barItemBezier.Tag = "BezierTool"
        Me.barItemBezier.Text = "Bezier"
        Me.barItemBezier.Tooltip = "BezierTool"
        '
        'barItemText
        '
        Me.barItemText.CategoryIndex = 1
        Me.barItemText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemText.ID = "Text"
        Me.barItemText.ImageIndex = 12
        Me.barItemText.ImageList = Me.smBarItemImages
        Me.barItemText.ShowToolTipInPopUp = False
        Me.barItemText.Tag = "TextTool"
        Me.barItemText.Text = "Text"
        Me.barItemText.Tooltip = "Text"
        '
        'barItemRichText
        '
        Me.barItemRichText.CategoryIndex = 1
        Me.barItemRichText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemRichText.ID = "Rich Text"
        Me.barItemRichText.ImageIndex = 47
        Me.barItemRichText.ImageList = Me.smBarItemImages
        Me.barItemRichText.ShowToolTipInPopUp = False
        Me.barItemRichText.Tag = "RichTextTool"
        Me.barItemRichText.Text = "Rich Text"
        Me.barItemRichText.Tooltip = "Rich Text"
        '
        'barItemImage
        '
        Me.barItemImage.CategoryIndex = 1
        Me.barItemImage.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemImage.ID = "Image"
        Me.barItemImage.ImageIndex = 11
        Me.barItemImage.ImageList = Me.smBarItemImages
        Me.barItemImage.ShowToolTipInPopUp = False
        Me.barItemImage.Tag = "ImageTool"
        Me.barItemImage.Text = "Image"
        Me.barItemImage.Tooltip = "Image"
        '
        'barNode
        '
        Me.barNode.BarName = "Node"
        Me.barNode.Caption = "Node"
        Me.barNode.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemGroup, Me.barItemUngroup, Me.barItemBringToFront, Me.barItemSendToBack, Me.barItemBringForward, Me.barItemSendBackward})
        Me.barNode.Manager = Me.childFrameBarManager
        '
        'barItemGroup
        '
        Me.barItemGroup.CategoryIndex = 2
        Me.barItemGroup.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemGroup.Enabled = False
        Me.barItemGroup.ID = "Group"
        Me.barItemGroup.ImageIndex = 13
        Me.barItemGroup.ImageList = Me.smBarItemImages
        Me.barItemGroup.ShowToolTipInPopUp = False
        Me.barItemGroup.Tag = "GroupTool"
        Me.barItemGroup.Text = "Group"
        Me.barItemGroup.Tooltip = "Group"
        '
        'barItemUngroup
        '
        Me.barItemUngroup.CategoryIndex = 2
        Me.barItemUngroup.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemUngroup.Enabled = False
        Me.barItemUngroup.ID = "Ungroup"
        Me.barItemUngroup.ImageIndex = 14
        Me.barItemUngroup.ImageList = Me.smBarItemImages
        Me.barItemUngroup.ShowToolTipInPopUp = False
        Me.barItemUngroup.Tag = "UngroupTool"
        Me.barItemUngroup.Text = "Ungroup"
        Me.barItemUngroup.Tooltip = "Ungroup"
        '
        'barItemBringToFront
        '
        Me.barItemBringToFront.CategoryIndex = 2
        Me.barItemBringToFront.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemBringToFront.ID = "Bring To Front"
        Me.barItemBringToFront.ImageIndex = 40
        Me.barItemBringToFront.ImageList = Me.smBarItemImages
        Me.barItemBringToFront.ShowToolTipInPopUp = False
        Me.barItemBringToFront.Text = "Bring To Front"
        Me.barItemBringToFront.Tooltip = "Bring To Front"
        '
        'barItemSendToBack
        '
        Me.barItemSendToBack.CategoryIndex = 2
        Me.barItemSendToBack.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSendToBack.ID = "Send To Back"
        Me.barItemSendToBack.ImageIndex = 42
        Me.barItemSendToBack.ImageList = Me.smBarItemImages
        Me.barItemSendToBack.ShowToolTipInPopUp = False
        Me.barItemSendToBack.Text = "Send To Back"
        '
        'barItemBringForward
        '
        Me.barItemBringForward.CategoryIndex = 2
        Me.barItemBringForward.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemBringForward.ID = "Bring Forward"
        Me.barItemBringForward.ImageIndex = 39
        Me.barItemBringForward.ImageList = Me.smBarItemImages
        Me.barItemBringForward.ShowToolTipInPopUp = False
        Me.barItemBringForward.Text = "Bring Forward"
        '
        'barItemSendBackward
        '
        Me.barItemSendBackward.CategoryIndex = 2
        Me.barItemSendBackward.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSendBackward.ID = "Send Backward"
        Me.barItemSendBackward.ImageIndex = 41
        Me.barItemSendBackward.ImageList = Me.smBarItemImages
        Me.barItemSendBackward.ShowToolTipInPopUp = False
        Me.barItemSendBackward.Text = "Send Backward"
        '
        'barNudge
        '
        Me.barNudge.BarName = "Nudge"
        Me.barNudge.Caption = "Nudge"
        Me.barNudge.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemNudgeUp, Me.barItemNudgeDown, Me.barItemNudgeLeft, Me.barItemNudgeRight})
        Me.barNudge.Manager = Me.childFrameBarManager
        '
        'barItemNudgeUp
        '
        Me.barItemNudgeUp.CategoryIndex = 4
        Me.barItemNudgeUp.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemNudgeUp.ID = "Nudge Up"
        Me.barItemNudgeUp.ImageIndex = 21
        Me.barItemNudgeUp.ImageList = Me.smBarItemImages
        Me.barItemNudgeUp.ShowToolTipInPopUp = False
        Me.barItemNudgeUp.Text = "Nudge Up"
        Me.barItemNudgeUp.Tooltip = "Nudge Up"
        '
        'barItemNudgeDown
        '
        Me.barItemNudgeDown.CategoryIndex = 4
        Me.barItemNudgeDown.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemNudgeDown.ID = "Nudge Down"
        Me.barItemNudgeDown.ImageIndex = 22
        Me.barItemNudgeDown.ImageList = Me.smBarItemImages
        Me.barItemNudgeDown.ShowToolTipInPopUp = False
        Me.barItemNudgeDown.Text = "Nudge Down"
        Me.barItemNudgeDown.Tooltip = "Nudge Down"
        '
        'barItemNudgeLeft
        '
        Me.barItemNudgeLeft.CategoryIndex = 4
        Me.barItemNudgeLeft.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemNudgeLeft.ID = "Nudge Left"
        Me.barItemNudgeLeft.ImageIndex = 19
        Me.barItemNudgeLeft.ImageList = Me.smBarItemImages
        Me.barItemNudgeLeft.ShowToolTipInPopUp = False
        Me.barItemNudgeLeft.Text = "Nudge Left"
        Me.barItemNudgeLeft.Tooltip = "Nudge Left"
        '
        'barItemNudgeRight
        '
        Me.barItemNudgeRight.CategoryIndex = 4
        Me.barItemNudgeRight.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemNudgeRight.ID = "Nudge Right"
        Me.barItemNudgeRight.ImageIndex = 20
        Me.barItemNudgeRight.ImageList = Me.smBarItemImages
        Me.barItemNudgeRight.ShowToolTipInPopUp = False
        Me.barItemNudgeRight.Text = "Nudge Right"
        Me.barItemNudgeRight.Tooltip = "Nudge Right"
        '
        'barLinks
        '
        Me.barLinks.BarName = "Links"
        Me.barLinks.Caption = "Links"
        Me.barLinks.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemLink, Me.barItemOrthogonalLink, Me.barItemDirectedLink})
        Me.barLinks.Manager = Me.childFrameBarManager
        '
        'barItemLink
        '
        Me.barItemLink.CategoryIndex = 3
        Me.barItemLink.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemLink.ID = "Link"
        Me.barItemLink.ImageIndex = 43
        Me.barItemLink.ImageList = Me.smBarItemImages
        Me.barItemLink.ShowToolTipInPopUp = False
        Me.barItemLink.Text = "Link"
        Me.barItemLink.Tooltip = "Link"
        '
        'barItemOrthogonalLink
        '
        Me.barItemOrthogonalLink.CategoryIndex = 3
        Me.barItemOrthogonalLink.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemOrthogonalLink.ID = "Orthogonal Link"
        Me.barItemOrthogonalLink.ImageIndex = 44
        Me.barItemOrthogonalLink.ImageList = Me.smBarItemImages
        Me.barItemOrthogonalLink.ShowToolTipInPopUp = False
        Me.barItemOrthogonalLink.Text = "Orthogonal Link"
        '
        'barItemDirectedLink
        '
        Me.barItemDirectedLink.CategoryIndex = 3
        Me.barItemDirectedLink.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemDirectedLink.ID = "Directed Link"
        Me.barItemDirectedLink.ImageIndex = 45
        Me.barItemDirectedLink.ImageList = Me.smBarItemImages
        Me.barItemDirectedLink.ShowToolTipInPopUp = False
        Me.barItemDirectedLink.Text = "Directed Link"
        Me.barItemDirectedLink.Tooltip = "Directed Link"
        '
        'barRotate
        '
        Me.barRotate.BarName = "Rotate"
        Me.barRotate.Caption = "Rotate"
        Me.barRotate.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemRotateLeft, Me.barItemRotateRight, Me.barItemFlipVertical, Me.barItemFlipHorizontal})
        Me.barRotate.Manager = Me.childFrameBarManager
        '
        'barItemRotateLeft
        '
        Me.barItemRotateLeft.CategoryIndex = 5
        Me.barItemRotateLeft.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemRotateLeft.ID = "Rotate Left"
        Me.barItemRotateLeft.ImageIndex = 35
        Me.barItemRotateLeft.ImageList = Me.smBarItemImages
        Me.barItemRotateLeft.ShowToolTipInPopUp = False
        Me.barItemRotateLeft.Text = "Rotate Left"
        Me.barItemRotateLeft.Tooltip = "Rotate Left"
        '
        'barItemRotateRight
        '
        Me.barItemRotateRight.CategoryIndex = 5
        Me.barItemRotateRight.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemRotateRight.ID = "Rotate Right"
        Me.barItemRotateRight.ImageIndex = 36
        Me.barItemRotateRight.ImageList = Me.smBarItemImages
        Me.barItemRotateRight.ShowToolTipInPopUp = False
        Me.barItemRotateRight.Text = "Rotate Right"
        Me.barItemRotateRight.Tooltip = "Rotate Right"
        '
        'barItemFlipVertical
        '
        Me.barItemFlipVertical.CategoryIndex = 5
        Me.barItemFlipVertical.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemFlipVertical.ID = "Flip Vertical"
        Me.barItemFlipVertical.ImageIndex = 38
        Me.barItemFlipVertical.ImageList = Me.smBarItemImages
        Me.barItemFlipVertical.ShowToolTipInPopUp = False
        Me.barItemFlipVertical.Text = "Flip Vertical"
        Me.barItemFlipVertical.Tooltip = "Flip Vertical"
        '
        'barItemFlipHorizontal
        '
        Me.barItemFlipHorizontal.CategoryIndex = 5
        Me.barItemFlipHorizontal.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemFlipHorizontal.ID = "Flip Horizontal"
        Me.barItemFlipHorizontal.ImageIndex = 37
        Me.barItemFlipHorizontal.ImageList = Me.smBarItemImages
        Me.barItemFlipHorizontal.ShowToolTipInPopUp = False
        Me.barItemFlipHorizontal.Text = "Flip Horizontal"
        Me.barItemFlipHorizontal.Tooltip = "Flip Horizontal"
        '
        'bar1
        '
        Me.bar1.BarName = "View"
        Me.bar1.Caption = "View"
        Me.bar1.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemPan, Me.barItemShowGrid, Me.barItemSnapToGrid, Me.barItemZoom, Me.comboBoxBarItemMagnification})
        Me.bar1.Manager = Me.childFrameBarManager
        '
        'barItemPan
        '
        Me.barItemPan.CategoryIndex = 6
        Me.barItemPan.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemPan.ID = "Pan"
        Me.barItemPan.ImageIndex = 15
        Me.barItemPan.ImageList = Me.smBarItemImages
        Me.barItemPan.ShowToolTipInPopUp = False
        Me.barItemPan.Tag = "PanTool"
        Me.barItemPan.Text = "Pan"
        Me.barItemPan.Tooltip = "Pan"
        '
        'barItemShowGrid
        '
        Me.barItemShowGrid.CategoryIndex = 6
        Me.barItemShowGrid.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemShowGrid.ID = "Show Grid"
        Me.barItemShowGrid.ImageIndex = 17
        Me.barItemShowGrid.ImageList = Me.smBarItemImages
        Me.barItemShowGrid.ShowToolTipInPopUp = False
        Me.barItemShowGrid.Text = "Show Grid"
        Me.barItemShowGrid.Tooltip = "Show Grid"
        '
        'barItemSnapToGrid
        '
        Me.barItemSnapToGrid.CategoryIndex = 6
        Me.barItemSnapToGrid.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSnapToGrid.ID = "Snap To Grid"
        Me.barItemSnapToGrid.ImageIndex = 18
        Me.barItemSnapToGrid.ImageList = Me.smBarItemImages
        Me.barItemSnapToGrid.ShowToolTipInPopUp = False
        Me.barItemSnapToGrid.Text = "Snap To Grid"
        Me.barItemSnapToGrid.Visible = False
        '
        'barItemZoom
        '
        Me.barItemZoom.CategoryIndex = 6
        Me.barItemZoom.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemZoom.ID = "Zoom"
        Me.barItemZoom.ImageIndex = 16
        Me.barItemZoom.ImageList = Me.smBarItemImages
        Me.barItemZoom.ShowToolTipInPopUp = False
        Me.barItemZoom.Tag = "ZoomTool"
        Me.barItemZoom.Text = "Zoom"
        Me.barItemZoom.Tooltip = "Zoom"
        '
        'comboBoxBarItemMagnification
        '
        Me.comboBoxBarItemMagnification.CategoryIndex = 6
        Me.comboBoxBarItemMagnification.ChoiceList.AddRange(New String() {"25%", "50%", "75%", "100%", "125%", "150%", "175%", "200%"})
        Me.comboBoxBarItemMagnification.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboBoxBarItemMagnification.Editable = False
        Me.comboBoxBarItemMagnification.ID = "Magnification"
        Me.comboBoxBarItemMagnification.ShowToolTipInPopUp = False
        Me.comboBoxBarItemMagnification.Text = "Magnification"
        Me.comboBoxBarItemMagnification.Tooltip = "Magnification"
        '
        'bar2
        '
        Me.bar2.BarName = "Text Formatting"
        Me.bar2.Caption = "Text Formatting"
        Me.bar2.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.comboBoxBarItemFontFamily, Me.comboBoxBarItemPointSize, Me.barItemBoldText, Me.barItemItalicText, Me.barItemUnderlineText, Me.barItemStrikeoutText, Me.barItemAlignTextLeft, Me.barItemCenterText, Me.barItemAlignTextRight, Me.barItemTextColor, Me.barItemSubscript, Me.barItemSuperscript, Me.barItemLower, Me.barItemUpper})
        Me.bar2.Manager = Me.childFrameBarManager
        '
        'comboBoxBarItemFontFamily
        '
        Me.comboBoxBarItemFontFamily.CategoryIndex = 7
        Me.comboBoxBarItemFontFamily.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboBoxBarItemFontFamily.Editable = False
        Me.comboBoxBarItemFontFamily.ID = "Font Family"
        Me.comboBoxBarItemFontFamily.MinWidth = 120
        Me.comboBoxBarItemFontFamily.ShowToolTipInPopUp = False
        Me.comboBoxBarItemFontFamily.Text = "Font Family"
        Me.comboBoxBarItemFontFamily.Tooltip = "Font Family"
        '
        'comboBoxBarItemPointSize
        '
        Me.comboBoxBarItemPointSize.CategoryIndex = 7
        Me.comboBoxBarItemPointSize.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.comboBoxBarItemPointSize.ID = "Point Size"
        Me.comboBoxBarItemPointSize.ShowToolTipInPopUp = False
        Me.comboBoxBarItemPointSize.Text = "Point Size"
        '
        'barItemBoldText
        '
        Me.barItemBoldText.CategoryIndex = 7
        Me.barItemBoldText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemBoldText.ID = "Bold"
        Me.barItemBoldText.ImageIndex = 48
        Me.barItemBoldText.ImageList = Me.smBarItemImages
        Me.barItemBoldText.Shortcut = System.Windows.Forms.Shortcut.CtrlB
        Me.barItemBoldText.ShowToolTipInPopUp = False
        Me.barItemBoldText.Text = "Bold"
        Me.barItemBoldText.Tooltip = "Bold"
        '
        'barItemItalicText
        '
        Me.barItemItalicText.CategoryIndex = 7
        Me.barItemItalicText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemItalicText.ID = "Italic"
        Me.barItemItalicText.ImageIndex = 49
        Me.barItemItalicText.ImageList = Me.smBarItemImages
        Me.barItemItalicText.Shortcut = System.Windows.Forms.Shortcut.CtrlI
        Me.barItemItalicText.ShowToolTipInPopUp = False
        Me.barItemItalicText.Text = "Italic"
        Me.barItemItalicText.Tooltip = "Italic"
        '
        'barItemUnderlineText
        '
        Me.barItemUnderlineText.CategoryIndex = 7
        Me.barItemUnderlineText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemUnderlineText.ID = "Underline"
        Me.barItemUnderlineText.ImageIndex = 50
        Me.barItemUnderlineText.ImageList = Me.smBarItemImages
        Me.barItemUnderlineText.Shortcut = System.Windows.Forms.Shortcut.CtrlU
        Me.barItemUnderlineText.ShowToolTipInPopUp = False
        Me.barItemUnderlineText.Text = "Underline"
        Me.barItemUnderlineText.Tooltip = "Underline"
        '
        'barItemStrikeoutText
        '
        Me.barItemStrikeoutText.CategoryIndex = 7
        Me.barItemStrikeoutText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemStrikeoutText.ID = "Strikeout"
        Me.barItemStrikeoutText.ImageIndex = 63
        Me.barItemStrikeoutText.ImageList = Me.smBarItemImages
        Me.barItemStrikeoutText.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.barItemStrikeoutText.ShowToolTipInPopUp = False
        Me.barItemStrikeoutText.Text = "Strikeout"
        Me.barItemStrikeoutText.Tooltip = "Strikeout"
        '
        'barItemAlignTextLeft
        '
        Me.barItemAlignTextLeft.CategoryIndex = 7
        Me.barItemAlignTextLeft.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignTextLeft.ID = "Align Text Left"
        Me.barItemAlignTextLeft.ImageIndex = 51
        Me.barItemAlignTextLeft.ImageList = Me.smBarItemImages
        Me.barItemAlignTextLeft.Shortcut = System.Windows.Forms.Shortcut.CtrlL
        Me.barItemAlignTextLeft.ShowToolTipInPopUp = False
        Me.barItemAlignTextLeft.Text = "Align Text Left"
        Me.barItemAlignTextLeft.Tooltip = "Align Text Left"
        '
        'barItemCenterText
        '
        Me.barItemCenterText.CategoryIndex = 7
        Me.barItemCenterText.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemCenterText.ID = "Center Text"
        Me.barItemCenterText.ImageIndex = 52
        Me.barItemCenterText.ImageList = Me.smBarItemImages
        Me.barItemCenterText.Shortcut = System.Windows.Forms.Shortcut.CtrlE
        Me.barItemCenterText.ShowToolTipInPopUp = False
        Me.barItemCenterText.Text = "Center Text"
        Me.barItemCenterText.Tooltip = "Center Text"
        '
        'barItemAlignTextRight
        '
        Me.barItemAlignTextRight.CategoryIndex = 7
        Me.barItemAlignTextRight.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignTextRight.ID = "Align Text Right"
        Me.barItemAlignTextRight.ImageIndex = 53
        Me.barItemAlignTextRight.ImageList = Me.smBarItemImages
        Me.barItemAlignTextRight.Shortcut = System.Windows.Forms.Shortcut.CtrlR
        Me.barItemAlignTextRight.ShowToolTipInPopUp = False
        Me.barItemAlignTextRight.Text = "Align Text Right"
        Me.barItemAlignTextRight.Tooltip = "Align Text Right"
        '
        'barItemTextColor
        '
        Me.barItemTextColor.CategoryIndex = 7
        Me.barItemTextColor.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemTextColor.ID = "Text Color"
        Me.barItemTextColor.ImageIndex = 54
        Me.barItemTextColor.ImageList = Me.smBarItemImages
        Me.barItemTextColor.ShowToolTipInPopUp = False
        Me.barItemTextColor.Text = "Text Color"
        Me.barItemTextColor.Tooltip = "Text Color"
        '
        'barItemSubscript
        '
        Me.barItemSubscript.CategoryIndex = 7
        Me.barItemSubscript.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSubscript.ID = "Subscript"
        Me.barItemSubscript.ImageIndex = 60
        Me.barItemSubscript.ImageList = Me.smBarItemImages
        Me.barItemSubscript.ShowToolTipInPopUp = False
        Me.barItemSubscript.Text = "Subscript"
        Me.barItemSubscript.Tooltip = "Subscript"
        '
        'barItemSuperscript
        '
        Me.barItemSuperscript.CategoryIndex = 7
        Me.barItemSuperscript.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSuperscript.ID = "Superscript"
        Me.barItemSuperscript.ImageIndex = 59
        Me.barItemSuperscript.ImageList = Me.smBarItemImages
        Me.barItemSuperscript.ShowToolTipInPopUp = False
        Me.barItemSuperscript.Text = "Superscript"
        Me.barItemSuperscript.Tooltip = "Superscript"
        '
        'barItemLower
        '
        Me.barItemLower.CategoryIndex = 7
        Me.barItemLower.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemLower.ID = "Lower"
        Me.barItemLower.ImageIndex = 62
        Me.barItemLower.ImageList = Me.smBarItemImages
        Me.barItemLower.ShowToolTipInPopUp = False
        Me.barItemLower.Text = "Lower"
        '
        'barItemUpper
        '
        Me.barItemUpper.CategoryIndex = 7
        Me.barItemUpper.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemUpper.ID = "Upper"
        Me.barItemUpper.ImageIndex = 61
        Me.barItemUpper.ImageList = Me.smBarItemImages
        Me.barItemUpper.ShowToolTipInPopUp = False
        Me.barItemUpper.Text = "Upper"
        '
        'bar3
        '
        Me.bar3.AllowHiding = False
        Me.bar3.BarName = "Scripting"
        Me.bar3.BarStyle = CType((Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.AllowQuickCustomizing Or Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle.DrawDragBorder), Syncfusion.Windows.Forms.Tools.XPMenus.BarStyle)
        Me.bar3.Caption = "Scripting"
        Me.bar3.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemLoadScript, Me.barItemRunScript, Me.barItemEditScript, Me.barItemStopScript})
        Me.bar3.Manager = Me.childFrameBarManager
        '
        'barItemLoadScript
        '
        Me.barItemLoadScript.CategoryIndex = 8
        Me.barItemLoadScript.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemLoadScript.ID = "Load Script"
        Me.barItemLoadScript.ImageIndex = 55
        Me.barItemLoadScript.ImageList = Me.smBarItemImages
        Me.barItemLoadScript.ShowToolTipInPopUp = False
        Me.barItemLoadScript.Text = "Load Script"
        Me.barItemLoadScript.Tooltip = "Load Script"
        '
        'barItemRunScript
        '
        Me.barItemRunScript.CategoryIndex = 8
        Me.barItemRunScript.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemRunScript.ID = "Run Script"
        Me.barItemRunScript.ImageIndex = 56
        Me.barItemRunScript.ImageList = Me.smBarItemImages
        Me.barItemRunScript.Shortcut = System.Windows.Forms.Shortcut.F5
        Me.barItemRunScript.ShowToolTipInPopUp = False
        Me.barItemRunScript.Text = "Run Script"
        Me.barItemRunScript.Tooltip = "Run Script"
        '
        'barItemEditScript
        '
        Me.barItemEditScript.CategoryIndex = 8
        Me.barItemEditScript.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemEditScript.ID = "Edit Script"
        Me.barItemEditScript.ImageIndex = 57
        Me.barItemEditScript.ImageList = Me.smBarItemImages
        Me.barItemEditScript.ShowToolTipInPopUp = False
        Me.barItemEditScript.Text = "Edit Script"
        Me.barItemEditScript.Tooltip = "Edit Script"
        '
        'barItemStopScript
        '
        Me.barItemStopScript.CategoryIndex = 8
        Me.barItemStopScript.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemStopScript.ID = "Stop Script"
        Me.barItemStopScript.ImageIndex = 58
        Me.barItemStopScript.ImageList = Me.smBarItemImages
        Me.barItemStopScript.Shortcut = System.Windows.Forms.Shortcut.CtrlF5
        Me.barItemStopScript.ShowToolTipInPopUp = False
        Me.barItemStopScript.Text = "Stop Script"
        Me.barItemStopScript.Tooltip = "Stop Script"
        '
        'barLayout
        '
        Me.barLayout.BarName = "Layout"
        Me.barLayout.Caption = "Layout"
        Me.barLayout.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemSpaceAcross, Me.barItemSpaceDown, Me.barItemSameWidth, Me.barItemSameHeight, Me.barItemSameSize})
        Me.barLayout.Manager = Me.childFrameBarManager
        '
        'barItemSpaceAcross
        '
        Me.barItemSpaceAcross.CategoryIndex = 10
        Me.barItemSpaceAcross.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSpaceAcross.ID = "Space Across"
        Me.barItemSpaceAcross.ImageIndex = 65
        Me.barItemSpaceAcross.ImageList = Me.smBarItemImages
        Me.barItemSpaceAcross.ShowToolTipInPopUp = False
        Me.barItemSpaceAcross.Text = "Space Across"
        '
        'barItemSpaceDown
        '
        Me.barItemSpaceDown.CategoryIndex = 10
        Me.barItemSpaceDown.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSpaceDown.ID = "Space Down"
        Me.barItemSpaceDown.ImageIndex = 66
        Me.barItemSpaceDown.ImageList = Me.smBarItemImages
        Me.barItemSpaceDown.ShowToolTipInPopUp = False
        Me.barItemSpaceDown.Text = "Space Down"
        '
        'barItemSameWidth
        '
        Me.barItemSameWidth.CategoryIndex = 10
        Me.barItemSameWidth.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSameWidth.ID = "Same Width"
        Me.barItemSameWidth.ImageIndex = 67
        Me.barItemSameWidth.ImageList = Me.smBarItemImages
        Me.barItemSameWidth.ShowToolTipInPopUp = False
        Me.barItemSameWidth.Text = "Same Width"
        '
        'barItemSameHeight
        '
        Me.barItemSameHeight.CategoryIndex = 10
        Me.barItemSameHeight.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSameHeight.ID = "Same Height"
        Me.barItemSameHeight.ImageIndex = 68
        Me.barItemSameHeight.ImageList = Me.smBarItemImages
        Me.barItemSameHeight.ShowToolTipInPopUp = False
        Me.barItemSameHeight.Text = "Same Height"
        '
        'barItemSameSize
        '
        Me.barItemSameSize.CategoryIndex = 10
        Me.barItemSameSize.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemSameSize.ID = "Same Size"
        Me.barItemSameSize.ImageIndex = 69
        Me.barItemSameSize.ImageList = Me.smBarItemImages
        Me.barItemSameSize.ShowToolTipInPopUp = False
        Me.barItemSameSize.Text = "Same Size"
        '
        'barAlign
        '
        Me.barAlign.BarName = "Align"
        Me.barAlign.Caption = "Align"
        Me.barAlign.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barItemAlignLeft, Me.barItemAlignCenter, Me.barItemAlignRight, Me.barItemAlignTop, Me.barItemAlignMiddle, Me.barItemAlignBottom})
        Me.barAlign.Manager = Me.childFrameBarManager
        '
        'barItemAlignLeft
        '
        Me.barItemAlignLeft.CategoryIndex = 9
        Me.barItemAlignLeft.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignLeft.ID = "Align Left"
        Me.barItemAlignLeft.ImageIndex = 70
        Me.barItemAlignLeft.ImageList = Me.smBarItemImages
        Me.barItemAlignLeft.ShowToolTipInPopUp = False
        Me.barItemAlignLeft.Text = "Align Left"
        '
        'barItemAlignCenter
        '
        Me.barItemAlignCenter.CategoryIndex = 9
        Me.barItemAlignCenter.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignCenter.ID = "Align Center"
        Me.barItemAlignCenter.ImageIndex = 71
        Me.barItemAlignCenter.ImageList = Me.smBarItemImages
        Me.barItemAlignCenter.ShowToolTipInPopUp = False
        Me.barItemAlignCenter.Text = "Align Center"
        '
        'barItemAlignRight
        '
        Me.barItemAlignRight.CategoryIndex = 9
        Me.barItemAlignRight.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignRight.ID = "Align Right"
        Me.barItemAlignRight.ImageIndex = 72
        Me.barItemAlignRight.ImageList = Me.smBarItemImages
        Me.barItemAlignRight.ShowToolTipInPopUp = False
        Me.barItemAlignRight.Text = "Align Right"
        '
        'barItemAlignTop
        '
        Me.barItemAlignTop.CategoryIndex = 9
        Me.barItemAlignTop.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignTop.ID = "Align Top"
        Me.barItemAlignTop.ImageIndex = 73
        Me.barItemAlignTop.ImageList = Me.smBarItemImages
        Me.barItemAlignTop.ShowToolTipInPopUp = False
        Me.barItemAlignTop.Text = "Align Top"
        '
        'barItemAlignMiddle
        '
        Me.barItemAlignMiddle.CategoryIndex = 9
        Me.barItemAlignMiddle.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignMiddle.ID = "Align Middle"
        Me.barItemAlignMiddle.ImageIndex = 74
        Me.barItemAlignMiddle.ImageList = Me.smBarItemImages
        Me.barItemAlignMiddle.ShowToolTipInPopUp = False
        Me.barItemAlignMiddle.Text = "Align Middle"
        '
        'barItemAlignBottom
        '
        Me.barItemAlignBottom.CategoryIndex = 9
        Me.barItemAlignBottom.CustomTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.barItemAlignBottom.ID = "Align Bottom"
        Me.barItemAlignBottom.ImageIndex = 75
        Me.barItemAlignBottom.ImageList = Me.smBarItemImages
        Me.barItemAlignBottom.ShowToolTipInPopUp = False
        Me.barItemAlignBottom.Text = "Align Bottom"
        '
        'barGeneral
        '
        Me.barGeneral.BarName = "General"
        Me.barGeneral.Caption = "General"
        Me.barGeneral.Items.AddRange(New Syncfusion.Windows.Forms.Tools.XPMenus.BarItem() {Me.barGuardar, Me.barGuardarComo, Me.barPageSetup, Me.barPrintPreview, Me.barImprimir})
        Me.barGeneral.Manager = Me.childFrameBarManager
        '
        'barGuardar
        '
        Me.barGuardar.CategoryIndex = 0
        Me.barGuardar.ID = "Guardar"
        Me.barGuardar.ImageIndex = 2
        Me.barGuardar.ImageList = Me.smallImageList
        Me.barGuardar.ShowToolTipInPopUp = False
        Me.barGuardar.Text = "Guardar"
        '
        'smallImageList
        '
        Me.smallImageList.ImageStream = CType(resources.GetObject("smallImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.smallImageList.TransparentColor = System.Drawing.Color.Fuchsia
        Me.smallImageList.Images.SetKeyName(0, "")
        Me.smallImageList.Images.SetKeyName(1, "")
        Me.smallImageList.Images.SetKeyName(2, "")
        Me.smallImageList.Images.SetKeyName(3, "")
        Me.smallImageList.Images.SetKeyName(4, "")
        Me.smallImageList.Images.SetKeyName(5, "")
        Me.smallImageList.Images.SetKeyName(6, "")
        Me.smallImageList.Images.SetKeyName(7, "")
        Me.smallImageList.Images.SetKeyName(8, "")
        Me.smallImageList.Images.SetKeyName(9, "")
        Me.smallImageList.Images.SetKeyName(10, "")
        Me.smallImageList.Images.SetKeyName(11, "")
        Me.smallImageList.Images.SetKeyName(12, "")
        Me.smallImageList.Images.SetKeyName(13, "")
        Me.smallImageList.Images.SetKeyName(14, "")
        Me.smallImageList.Images.SetKeyName(15, "")
        Me.smallImageList.Images.SetKeyName(16, "")
        Me.smallImageList.Images.SetKeyName(17, "")
        Me.smallImageList.Images.SetKeyName(18, "")
        Me.smallImageList.Images.SetKeyName(19, "")
        Me.smallImageList.Images.SetKeyName(20, "")
        Me.smallImageList.Images.SetKeyName(21, "")
        Me.smallImageList.Images.SetKeyName(22, "")
        Me.smallImageList.Images.SetKeyName(23, "")
        Me.smallImageList.Images.SetKeyName(24, "")
        Me.smallImageList.Images.SetKeyName(25, "")
        Me.smallImageList.Images.SetKeyName(26, "")
        Me.smallImageList.Images.SetKeyName(27, "")
        Me.smallImageList.Images.SetKeyName(28, "")
        Me.smallImageList.Images.SetKeyName(29, "")
        Me.smallImageList.Images.SetKeyName(30, "")
        Me.smallImageList.Images.SetKeyName(31, "")
        Me.smallImageList.Images.SetKeyName(32, "")
        Me.smallImageList.Images.SetKeyName(33, "")
        '
        'barGuardarComo
        '
        Me.barGuardarComo.CategoryIndex = 0
        Me.barGuardarComo.ID = "GuardarComo"
        Me.barGuardarComo.ImageIndex = 2
        Me.barGuardarComo.ImageList = Me.smallImageList
        Me.barGuardarComo.ShowToolTipInPopUp = False
        Me.barGuardarComo.Text = "Guardar Como..."
        '
        'barPageSetup
        '
        Me.barPageSetup.CategoryIndex = 0
        Me.barPageSetup.ID = "PageSetup"
        Me.barPageSetup.ImageIndex = 5
        Me.barPageSetup.ImageList = Me.smallImageList
        Me.barPageSetup.ShowToolTipInPopUp = False
        Me.barPageSetup.Text = "PageSetup"
        '
        'barPrintPreview
        '
        Me.barPrintPreview.CategoryIndex = 0
        Me.barPrintPreview.ID = "PrintPreview"
        Me.barPrintPreview.ImageIndex = 3
        Me.barPrintPreview.ImageList = Me.smallImageList
        Me.barPrintPreview.ShowToolTipInPopUp = False
        Me.barPrintPreview.Text = "PrintPreview"
        '
        'barImprimir
        '
        Me.barImprimir.CategoryIndex = 0
        Me.barImprimir.ID = "Imprimir"
        Me.barImprimir.ImageIndex = 6
        Me.barImprimir.ImageList = Me.smallImageList
        Me.barImprimir.ShowToolTipInPopUp = False
        Me.barImprimir.Text = "Imprimir"
        '
        'superToolTip1
        '
        Me.superToolTip1.InitialDelay = 1000
        Me.superToolTip1.ToolTipDuration = 3
        '
        'openDiagramDialog
        '
        Me.openDiagramDialog.Filter = "Diagram Files|*.edd|All files|*.*"
        Me.openDiagramDialog.Title = "Open Diagram"
        '
        'saveDiagramDialog
        '
        Me.saveDiagramDialog.FileName = "doc1"
        Me.saveDiagramDialog.Filter = "Diagram files|*.edd|EMF file|*.emf|GIF file|*.gif|PNG file|*.png|BMP file|*.bmp|J" & _
    "PEG file|*.jpeg,*.jpg|TIFF file|*.tiff|SVG file|*.svg|All files|*.*"
        '
        'GRD_Linea
        '
        Me.GRD_Linea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Linea.Location = New System.Drawing.Point(128, 35)
        Me.GRD_Linea.Name = "GRD_Linea"
        Me.GRD_Linea.pAccessibleName = Nothing
        Me.GRD_Linea.pActivarBotonFiltro = False
        Me.GRD_Linea.pText = " "
        Me.GRD_Linea.Size = New System.Drawing.Size(719, 184)
        Me.GRD_Linea.TabIndex = 5
        '
        'C_Familia
        '
        Me.C_Familia.Location = New System.Drawing.Point(302, 9)
        Me.C_Familia.Name = "C_Familia"
        Me.C_Familia.Size = New System.Drawing.Size(225, 21)
        Me.C_Familia.TabIndex = 119
        Me.C_Familia.Text = "C_Familia"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(245, 13)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(67, 16)
        Me.UltraLabel3.TabIndex = 120
        Me.UltraLabel3.Text = "Familia:"
        '
        'UltraPanel1
        '
        Me.UltraPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        'UltraPanel1.ClientArea
        '
        Me.UltraPanel1.ClientArea.Controls.Add(Me.SplitContainer1)
        Me.UltraPanel1.Location = New System.Drawing.Point(128, 214)
        Me.UltraPanel1.Name = "UltraPanel1"
        Me.UltraPanel1.Size = New System.Drawing.Size(719, 330)
        Me.UltraPanel1.TabIndex = 125
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.diagramComponent)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.propertyEditor1)
        Me.SplitContainer1.Size = New System.Drawing.Size(719, 330)
        Me.SplitContainer1.SplitterDistance = 564
        Me.SplitContainer1.TabIndex = 10
        '
        'diagramComponent
        '
        Me.diagramComponent.Controller.PasteOffset = New System.Drawing.SizeF(10.0!, 10.0!)
        Me.diagramComponent.DefaultContextMenuEnabled = False
        Me.diagramComponent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.diagramComponent.HScroll = True
        Me.diagramComponent.LayoutManager = Nothing
        Me.diagramComponent.Location = New System.Drawing.Point(0, 0)
        Me.diagramComponent.Model = Nothing
        Me.diagramComponent.Name = "diagramComponent"
        Me.diagramComponent.ScrollVirtualBounds = CType(resources.GetObject("diagramComponent.ScrollVirtualBounds"), System.Drawing.RectangleF)
        Me.diagramComponent.Size = New System.Drawing.Size(564, 330)
        Me.diagramComponent.SmartSizeBox = False
        Me.diagramComponent.TabIndex = 132
        '
        '
        '
        Me.diagramComponent.View.ClientRectangle = New System.Drawing.Rectangle(0, 0, 0, 0)
        Me.diagramComponent.View.Controller = Me.diagramComponent.Controller
        Me.diagramComponent.View.Grid.MinPixelSpacing = 4.0!
        Me.diagramComponent.View.ScrollVirtualBounds = CType(resources.GetObject("resource.ScrollVirtualBounds"), System.Drawing.RectangleF)
        Me.diagramComponent.VScroll = True
        '
        'propertyEditor1
        '
        Me.propertyEditor1.Diagram = Me.diagramComponent
        Me.propertyEditor1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.propertyEditor1.Location = New System.Drawing.Point(0, 0)
        Me.propertyEditor1.Name = "propertyEditor1"
        Me.propertyEditor1.Size = New System.Drawing.Size(151, 330)
        Me.propertyEditor1.TabIndex = 12
        '
        'paletteGroupBar1
        '
        Me.paletteGroupBar1.AllowDrop = True
        Me.paletteGroupBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.paletteGroupBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.paletteGroupBar1.Controls.Add(Me.paletteGroupView1)
        Me.paletteGroupBar1.Diagram = Me.diagramComponent
        Me.paletteGroupBar1.Dock = System.Windows.Forms.DockStyle.Left
        Me.paletteGroupBar1.EditMode = False
        Me.paletteGroupBar1.ExpandButtonToolTip = Nothing
        Me.paletteGroupBar1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.paletteGroupBar1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(140, Byte), Integer))
        Me.paletteGroupBar1.GroupBarDropDownToolTip = Nothing
        Me.paletteGroupBar1.GroupBarItems.AddRange(New Syncfusion.Windows.Forms.Tools.GroupBarItem() {Me.GroupBarItem1})
        Me.paletteGroupBar1.IconRenderingMode = Syncfusion.Windows.Forms.Tools.IconRenderingMode.[Default]
        Me.paletteGroupBar1.IndexOnVisibleItems = True
        Me.paletteGroupBar1.Location = New System.Drawing.Point(0, 0)
        Me.paletteGroupBar1.MinimizeButtonToolTip = Nothing
        Me.paletteGroupBar1.Name = "paletteGroupBar1"
        Me.paletteGroupBar1.NavigationPaneTooltip = Nothing
        Me.paletteGroupBar1.PopupClientSize = New System.Drawing.Size(0, 0)
        Me.paletteGroupBar1.SelectedItem = 0
        Me.paletteGroupBar1.Size = New System.Drawing.Size(128, 545)
        Me.paletteGroupBar1.TabIndex = 130
        Me.paletteGroupBar1.Text = "paletteGroupBar1"
        Me.paletteGroupBar1.VisualStyle = Syncfusion.Windows.Forms.VisualStyle.VS2005
        '
        'paletteGroupView1
        '
        Me.paletteGroupView1.BackColor = System.Drawing.Color.White
        Me.paletteGroupView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.paletteGroupView1.ButtonView = True
        Me.paletteGroupView1.Diagram = Me.diagramComponent
        Me.paletteGroupView1.Location = New System.Drawing.Point(1, 23)
        Me.paletteGroupView1.Name = "paletteGroupView1"
        Me.paletteGroupView1.SelectingItemColor = System.Drawing.Color.FromArgb(CType(CType(218, Byte), Integer), CType(CType(231, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.paletteGroupView1.Size = New System.Drawing.Size(126, 521)
        Me.paletteGroupView1.TabIndex = 2
        Me.paletteGroupView1.Text = "paletteGroupView1"
        '
        'GroupBarItem1
        '
        Me.GroupBarItem1.BackColor = System.Drawing.Color.FromArgb(CType(CType(191, Byte), Integer), CType(CType(219, Byte), Integer), CType(CType(254, Byte), Integer))
        Me.GroupBarItem1.Client = Me.paletteGroupView1
        Me.GroupBarItem1.Text = "Formas"
        '
        'L_NotaInformativa
        '
        Me.L_NotaInformativa.Location = New System.Drawing.Point(533, 14)
        Me.L_NotaInformativa.Name = "L_NotaInformativa"
        Me.L_NotaInformativa.Size = New System.Drawing.Size(447, 21)
        Me.L_NotaInformativa.TabIndex = 135
        Me.L_NotaInformativa.Text = "Sólo se visualizarán los productos que tengan el identificador del producto intro" & _
    "ducido"
        '
        'frmPlano
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(847, 545)
        Me.Controls.Add(Me.L_NotaInformativa)
        Me.Controls.Add(Me.paletteGroupBar1)
        Me.Controls.Add(Me.UltraPanel1)
        Me.Controls.Add(Me.GRD_Linea)
        Me.Controls.Add(Me.C_Familia)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmPlano"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Diagram"
        Me.Controls.SetChildIndex(Me.UltraLabel3, 0)
        Me.Controls.SetChildIndex(Me.C_Familia, 0)
        Me.Controls.SetChildIndex(Me.GRD_Linea, 0)
        Me.Controls.SetChildIndex(Me.UltraPanel1, 0)
        Me.Controls.SetChildIndex(Me.paletteGroupBar1, 0)
        Me.Controls.SetChildIndex(Me.L_NotaInformativa, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        CType(Me.document, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.childFrameBarManager, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.comboBoxBarItemMagnification, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.comboBoxBarItemFontFamily, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.comboBoxBarItemPointSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Familia, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraPanel1.ClientArea.ResumeLayout(False)
        Me.UltraPanel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.diagramComponent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.paletteGroupBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.paletteGroupBar1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

#Region "Class events"

#Region "DiagramForm"

    Private Sub frmPlano_Deactivate(sender As Object, e As System.EventArgs) Handles Me.Deactivate

    End Sub

    Private Sub frmPlano_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Call barGuardar_Click(Nothing, Nothing)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the Load event of the DiagramForm control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    Private Sub DiagramForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.barItemShowGrid.Checked = Me.diagramComponent.View.Grid.Visible
        Me.barItemSnapToGrid.Checked = Me.diagramComponent.View.Grid.SnapToGrid
        UpdateMagnificationComboBox()

        ' Set Active SelectTool
        Me.ActiveToolBarItem = Me.barItemSelect

        ' Populate Tools Hashtable
        Me.Tools.Add("SelectTool", Me.barItemSelect)
        Me.Tools.Add("RectangleTool", Me.barItemRectangle)
        Me.Tools.Add("BezierTool", Me.barItemBezier)

        Me.Tools.Add("PencilTool", Me.barItemPencil)
        Me.Tools.Add("SplineTool", Me.barItemSpline)
        Me.Tools.Add("BitmapTool", Me.barItemImage)
        Me.Tools.Add("ClosedCurveTool", Me.barItemClosedCurve)
        Me.Tools.Add("CurveTool", Me.barItemCurve)
        Me.Tools.Add("EllipseTool", Me.barItemEllipse)
        Me.Tools.Add("RoundRectTool", Me.barItemRoundRect)
        Me.Tools.Add("LineTool", Me.barItemLine)
        Me.Tools.Add("RichTextTool", Me.barItemRichText)
        Me.Tools.Add("TextTool", Me.barItemText)
        Me.Tools.Add("ZoomTool", Me.barItemZoom)
        Me.Tools.Add("PanTool", Me.barItemPan)
        Me.Tools.Add("PolygonTool", Me.barItemPolygon)
        Me.Tools.Add("PolyLineTool", Me.barItemPolyline)
        Me.Tools.Add("LineLinkTool", Me.barItemLink)
        Me.Tools.Add("OrthogonalLinkTool", Me.barItemOrthogonalLink)
        Me.Tools.Add("DirectedLineLinkTool", Me.barItemDirectedLink)

        SetActiveTool(CType(Nothing, Tool))

        AddHandler diagramComponent.EventSink.NodeCollectionChanged, AddressOf OnSelectionChanged
        AddHandler diagramComponent.EventSink.PropertyChanged, AddressOf View_PropertyChanged

        ' Set focus to the diagram control
        diagramComponent.Focus()

        ' To add a toolTip through code
        toolTipInfo = New Syncfusion.Windows.Forms.Tools.ToolTipInfo()
        ' Customize SuperToolTip values..
        toolTipInfo.BackColor = SystemColors.Control
        toolTipInfo.Body.Text = "Set EnableCentralPort property of the Node to True," & Constants.vbCrLf & "to establish connection between nodes"
        toolTipInfo.Header.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte(0)))
        toolTipInfo.Header.Text = "Need To Enable Node's CentralPort !!!"
        toolTipInfo.Header.TextAlign = ContentAlignment.MiddleCenter
    End Sub

    <EventHandlerPriorityAttribute(True)> _
    Private Sub DiagramForm_ToolActivated(ByVal evtArgs As ToolEventArgs)
        If Not evtArgs.Tool.Name Is Nothing Then
            Dim strTool As String = evtArgs.Tool.Name

            If Not strTool Is Nothing AndAlso Me.Tools.Contains(strTool) Then
                Me.ActiveToolBarItem = CType(IIf(TypeOf Me.Tools(strTool) Is BarItem, Me.Tools(strTool), Nothing), BarItem)
            End If
            If strTool.Equals("LineLinkTool") OrElse strTool.Equals("DirectedLineLinkTool") OrElse (strTool.Equals("OrthogonalLinkTool")) Then
                Me.superToolTip1.Show(Me.toolTipInfo, New Point(MousePosition.X, MousePosition.Y), 3000)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Handles the Closing event of the DiagramForm control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
    Private Sub DiagramForm_Closing(ByVal sender As Object, ByVal e As CancelEventArgs) Handles MyBase.Closing
        Dim propEditor As PropertyEditor = Me.PropertyEditor
        If Not propEditor Is Nothing Then
            propEditor.Diagram = Nothing
        End If
    End Sub


    <EventHandlerPriorityAttribute(True)> _
    Protected Sub OnSelectionChanged(ByVal evtArgs As CollectionExEventArgs)
        Dim controller As DiagramController = Me.diagramComponent.Controller

        If Not controller Is Nothing Then
            If Not controller.SelectionList Is Nothing Then
                ' Check for grouping and ungrouping tools available.
                Dim bGroupAvailable As Boolean = False
                Dim bGroupPresent As Boolean = False

                If controller.View.SelectionList.Count >= 2 Then
                    bGroupAvailable = True
                End If

                For Each curNode As Node In controller.View.SelectionList
                    ' Check for groups.
                    If TypeOf curNode Is Group Then
                        bGroupPresent = True
                        Exit For
                    End If
                Next curNode

                ' Update group and ungroup tools.
                Me.barItemGroup.Enabled = bGroupAvailable
                Me.barItemUngroup.Enabled = bGroupPresent

                Dim fmtSelection As SelectionFormat = controller.TextEditor.GetSelectionFormat(False)

                If fmtSelection.Valid Then
                    ' 1 - FontStyles
                    ' Bold
                    If ((fmtSelection.FontStyle And System.Drawing.FontStyle.Bold) = System.Drawing.FontStyle.Bold) Then
                        barItemBoldText.Checked = True
                    Else
                        barItemBoldText.Checked = False
                    End If
                    ' Underline
                    If ((fmtSelection.FontStyle And System.Drawing.FontStyle.Underline) = System.Drawing.FontStyle.Underline) Then
                        barItemUnderlineText.Checked = True
                    Else
                        barItemUnderlineText.Checked = False
                    End If
                    ' Italic
                    If ((fmtSelection.FontStyle And System.Drawing.FontStyle.Italic) = System.Drawing.FontStyle.Italic) Then
                        barItemItalicText.Checked = True
                    Else
                        barItemItalicText.Checked = False
                    End If
                    ' Strikeout
                    If ((fmtSelection.FontStyle And System.Drawing.FontStyle.Strikeout) = System.Drawing.FontStyle.Strikeout) Then
                        barItemStrikeoutText.Checked = True
                    Else
                        barItemStrikeoutText.Checked = False
                    End If

                    ' 2 - Alignment
                    Select Case fmtSelection.Alignment
                        Case StringAlignment.Near
                            Me.CurrentAlignment = Me.barItemAlignTextLeft
                        Case StringAlignment.Center
                            Me.CurrentAlignment = Me.barItemCenterText
                        Case StringAlignment.Far
                            Me.CurrentAlignment = Me.barItemAlignTextRight
                    End Select

                    ' 3 - FamilyName
                    Me.comboBoxBarItemFontFamily.TextBoxValue = fmtSelection.FontFamily

                    ' 4 - FontHeight
                    If fmtSelection.FontHeight = 0 Then
                        Me.comboBoxBarItemPointSize.TextBoxValue = String.Empty
                    Else
                        Me.comboBoxBarItemPointSize.TextBoxValue = fmtSelection.FontHeight.ToString()
                    End If
                Else
                    barItemBoldText.Checked = False
                    barItemItalicText.Checked = False
                    barItemUnderlineText.Checked = False
                    barItemStrikeoutText.Checked = False
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Updates the magnification combo box to view zoom factor.
    ''' </summary>
    Private Sub UpdateMagnificationComboBox()
        Me.comboBoxBarItemMagnification.TextBoxValue = Me.diagramComponent.Magnification & "%"
    End Sub
    ''' <summary>
    ''' Updates the text formatting for selected node.
    ''' </summary>
    Private Sub UpdateUITextFormatting()
        Dim controller As DiagramController = Me.diagramComponent.Controller

        If controller Is Nothing OrElse controller.TextEditor Is Nothing Then
            Return
        End If

        ' Update FamilyName
        Me.comboBoxBarItemFontFamily.TextBoxValue = controller.TextEditor.FamilyName

        Me.barItemBoldText.Checked = controller.TextEditor.Bold
        Me.barItemItalicText.Checked = controller.TextEditor.Italic
        Me.barItemUnderlineText.Checked = controller.TextEditor.Underline

        ' Update point size bar item
        Dim ptSize As Single = controller.TextEditor.PointSize

        If ptSize = 0 Then
            Me.comboBoxBarItemPointSize.TextBoxValue = String.Empty
        Else
            Me.comboBoxBarItemPointSize.TextBoxValue = ptSize.ToString()
        End If

        ' Update strikeout, superscript, subscript
        Me.barItemStrikeoutText.Checked = controller.TextEditor.Strikeout
        Me.barItemSubscript.Checked = controller.TextEditor.Subscript
        Me.barItemSuperscript.Checked = controller.TextEditor.Superscript

        ' Update text alignment bar items
        Dim horzAlign As StringAlignment = controller.TextEditor.HorizontalAlignment

        Select Case horzAlign
            Case StringAlignment.Near
                Me.CurrentAlignment = Me.barItemAlignTextLeft
            Case StringAlignment.Center
                Me.CurrentAlignment = Me.barItemCenterText
            Case StringAlignment.Far
                Me.CurrentAlignment = Me.barItemAlignTextRight
        End Select
    End Sub

    ''' <summary>
    ''' Loads the font for selection nodes.
    ''' </summary>
    Private Sub LoadFontSelections()
        ' Create ListBox containing names of font families and attach it to the
        ' font family combo box bar item
        Me.comboBoxBarItemFontFamily.ListBox = New ListBox()
        For Each curFontFamily As FontFamily In FontFamily.Families
            Me.comboBoxBarItemFontFamily.ListBox.Items.Add(curFontFamily.Name)
        Next curFontFamily
        AddHandler comboBoxBarItemFontFamily.ListBox.SelectedIndexChanged, AddressOf FontFamily_SelectedIndexChanged

        ' Create ListBox containing point sizes and attach to the point size combo
        ' box bar item
        Dim pointSizeListBox As ListBox = New ListBox()
        Dim ptSizes As Integer() = New Integer() {8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72}
        For Each ptSize As Integer In ptSizes
            pointSizeListBox.Items.Add(ptSize)
        Next ptSize
        Me.comboBoxBarItemPointSize.ListBox = pointSizeListBox
        AddHandler comboBoxBarItemPointSize.ListBox.SelectedIndexChanged, AddressOf PointSize_SelectedIndexChanged
    End Sub

    ''' <summary>
    ''' Get on current selecton list has textBoxNodes.
    ''' </summary>
    ''' <returns>true - one or more text nodes; false - none</returns>
    Private Function CheckTextSelecionNode() As Boolean
        Dim bResult As Boolean = False

        If diagramComponent.Controller Is Nothing Then
            Return bResult
        End If

        If diagramComponent.Controller.TextEditor.IsEditing Then
            bResult = True
        Else
            Dim selectionNodes As NodeCollection = Me.diagramComponent.Controller.SelectionList

            If Not selectionNodes Is Nothing Then
                For Each node As INode In selectionNodes
                    If TypeOf node Is TextNode Then
                        bResult = True
                        Exit For
                    End If
                Next node
            End If
        End If

        Return bResult
    End Function
#End Region

    Private Sub barItemLinkSymbols_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemLink.Click
        SetActiveTool("LineLinkTool")

        Dim barItemToSelect As BarItem = CType(IIf(TypeOf sender Is BarItem, sender, Nothing), BarItem)

        If Not barItemToSelect Is Nothing Then
            Me.ActiveToolBarItem = barItemToSelect
        End If
    End Sub

    Private Sub barItemShowGrid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemShowGrid.Click
        Me.barItemShowGrid.Checked = Not Me.barItemShowGrid.Checked
        Me.diagramComponent.View.Grid.Visible = Me.barItemShowGrid.Checked
        Me.diagramComponent.Invalidate(True)
    End Sub

    Private Sub barItemSnapToGrid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemSnapToGrid.Click
        Me.barItemSnapToGrid.Checked = Not Me.barItemSnapToGrid.Checked
        Me.diagramComponent.View.Grid.SnapToGrid = Me.barItemSnapToGrid.Checked
    End Sub

    Private Sub comboBoxBarItemMagnification_Click(ByVal sender As Object, ByVal e As EventArgs) Handles comboBoxBarItemMagnification.Click
        Dim strMagValue As String = Me.comboBoxBarItemMagnification.TextBoxValue
        Dim idxPctSign As Integer = strMagValue.IndexOf("%"c)
        If idxPctSign >= 0 Then
            strMagValue = strMagValue.Remove(idxPctSign, 1)
        End If
        Dim magVal As Integer = Convert.ToInt32(strMagValue)
        Me.diagramComponent.View.Magnification = magVal
    End Sub
    Private Sub barItemBringToFront_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuOrdBTF.Click, barItemBringToFront.Click
        Me.diagramComponent.Controller.BringToFront()
    End Sub

    Private Sub barItemSendToBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuOrdSTB.Click, barItemSendToBack.Click
        Me.diagramComponent.Controller.SendToBack()
    End Sub

    Private Sub barItemBringForward_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuOrdBF.Click, barItemBringForward.Click
        Me.diagramComponent.Controller.BringForward()
    End Sub

    Private Sub barItemSendBackward_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuOrdSB.Click, barItemSendBackward.Click
        Me.diagramComponent.Controller.SendBackward()
    End Sub

    Private Sub barItemNudgeUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemNudgeUp.Click
        Me.diagramComponent.NudgeUp()
    End Sub

    Private Sub barItemNudgeDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemNudgeDown.Click
        Me.diagramComponent.NudgeDown()
    End Sub

    Private Sub barItemNudgeLeft_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemNudgeLeft.Click
        Me.diagramComponent.NudgeLeft()
    End Sub

    Private Sub barItemNudgeRight_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemNudgeRight.Click
        Me.diagramComponent.NudgeRight()
    End Sub

    Private Sub barItemRotateLeft_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRtCClockwise.Click, barItemRotateLeft.Click
        Me.diagramComponent.Rotate(-90)
    End Sub

    Private Sub barItemRotateRight_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRtClockwise.Click, barItemRotateRight.Click
        Me.diagramComponent.Rotate(90)
    End Sub

    Private Sub barItemFlipVertical_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFlipVert.Click, barItemFlipVertical.Click
        Me.diagramComponent.FlipVertical()
    End Sub
    Private Sub barItemFlipHorizontal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFlipHoriz.Click, barItemFlipHorizontal.Click
        Me.diagramComponent.FlipHorizontal()
    End Sub

    Private Sub barItemImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemImage.Click
        SetActiveTool("BitmapTool")
    End Sub

    Private Sub barItemOrthogonalLink_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemOrthogonalLink.Click
        SetActiveTool("OrthogonalLinkTool")

        Dim barItemToSelect As BarItem = CType(IIf(TypeOf sender Is BarItem, sender, Nothing), BarItem)

        If Not barItemToSelect Is Nothing Then
            Me.ActiveToolBarItem = barItemToSelect
        End If
    End Sub

    Private Sub barItemDirectedLink_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemDirectedLink.Click
        SetActiveTool("DirectedLineLinkTool")

        Dim barItemToSelect As BarItem = CType(IIf(TypeOf sender Is BarItem, sender, Nothing), BarItem)

        If Not barItemToSelect Is Nothing Then
            Me.ActiveToolBarItem = barItemToSelect
        End If
    End Sub

    Private Sub FontFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim strFamilyName As String = Me.comboBoxBarItemFontFamily.ListBox.SelectedItem.ToString()

        If Me.diagramComponent.Controller.TextEditor.FamilyName <> strFamilyName Then
            Me.diagramComponent.Controller.TextEditor.FamilyName = strFamilyName
        End If
    End Sub
    Private Sub PointSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim selectedIdx As Integer = Me.comboBoxBarItemPointSize.ListBox.SelectedIndex
        If selectedIdx >= 0 Then
            Dim ptSize As Integer = CInt(Me.comboBoxBarItemPointSize.ListBox.Items(selectedIdx))
            Me.diagramComponent.Controller.TextEditor.PointSize = ptSize
        End If
    End Sub

    Private Sub barItemBoldText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemBoldText.Click
        If (Not CheckTextSelecionNode()) Then
            Return
        End If

        Dim newValue As Boolean = Not (Me.diagramComponent.Controller.TextEditor.Bold)
        Me.diagramComponent.Controller.TextEditor.Bold = newValue
        Me.barItemBoldText.Checked = newValue
    End Sub
    Private Sub barItemItalicText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemItalicText.Click
        If (Not CheckTextSelecionNode()) Then
            Return
        End If

        Dim newValue As Boolean = Not (Me.diagramComponent.Controller.TextEditor.Italic)
        Me.diagramComponent.Controller.TextEditor.Italic = newValue
        Me.barItemItalicText.Checked = newValue
    End Sub
    Private Sub barItemUnderlineText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemUnderlineText.Click
        If (Not CheckTextSelecionNode()) Then
            Return
        End If

        Dim newValue As Boolean = Not (Me.diagramComponent.Controller.TextEditor.Underline)
        Me.diagramComponent.Controller.TextEditor.Underline = newValue
        Me.barItemUnderlineText.Checked = newValue
    End Sub
    Private Sub barItemStrikeoutText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemStrikeoutText.Click
        If (Not CheckTextSelecionNode()) Then
            Return
        End If

        Dim newValue As Boolean = Not (Me.diagramComponent.Controller.TextEditor.Strikeout)
        Me.diagramComponent.Controller.TextEditor.Strikeout = newValue
        Me.barItemStrikeoutText.Checked = newValue
    End Sub
    Private Sub barItemTextColor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemTextColor.Click
        Dim dlg As ColorDialog = New ColorDialog()
        dlg.Color = Me.diagramComponent.Controller.TextEditor.TextColor
        If dlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Me.diagramComponent.Controller.TextEditor.TextColor = dlg.Color
        End If
    End Sub
    Private Sub barItemAlignTextLeft_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemAlignTextLeft.Click
        Me.diagramComponent.Controller.TextEditor.HorizontalAlignment = StringAlignment.Near
        Me.barItemAlignTextLeft.Checked = True
        Me.barItemCenterText.Checked = False
        Me.barItemAlignTextRight.Checked = False
    End Sub
    Private Sub barItemCenterText_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemCenterText.Click
        Me.diagramComponent.Controller.TextEditor.HorizontalAlignment = StringAlignment.Center
        Me.barItemCenterText.Checked = True
        Me.barItemAlignTextLeft.Checked = False
        Me.barItemAlignTextRight.Checked = False
    End Sub
    Private Sub barItemAlignTextRight_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemAlignTextRight.Click
        Me.diagramComponent.Controller.TextEditor.HorizontalAlignment = StringAlignment.Far
        Me.barItemAlignTextRight.Checked = True
        Me.barItemAlignTextLeft.Checked = False
        Me.barItemCenterText.Checked = False
    End Sub
    Private Sub barItemSubscript_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemSubscript.Click
        Dim newValue As Boolean = Not (Me.diagramComponent.Controller.TextEditor.Subscript)
        Me.diagramComponent.Controller.TextEditor.Subscript = newValue
    End Sub
    Private Sub barItemSuperscript_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemSuperscript.Click
        Dim newValue As Boolean = Not (Me.diagramComponent.Controller.TextEditor.Superscript)
        Me.diagramComponent.Controller.TextEditor.Superscript = newValue
    End Sub
    Private Sub barItemLower_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemLower.Click
        Dim nCurrentOffset As Integer = Me.diagramComponent.Controller.TextEditor.CharOffset
        nCurrentOffset -= 1
        Me.diagramComponent.Controller.TextEditor.CharOffset = nCurrentOffset
    End Sub
    Private Sub barItemUpper_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemUpper.Click
        Dim nCurrentOffset As Integer = Me.diagramComponent.Controller.TextEditor.CharOffset
        nCurrentOffset += 1
        Me.diagramComponent.Controller.TextEditor.CharOffset = nCurrentOffset
    End Sub

    Private Sub barItemGroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuGGroup.Click, barItemGroup.Click
        diagramComponent.Controller.Group()
    End Sub

    Private Sub barItemUngroup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuGUngroup.Click, barItemUngroup.Click
        diagramComponent.Controller.UnGroup()
    End Sub

    Private Sub barItemPan_Click(ByVal sender As Object, ByVal e As EventArgs) Handles barItemPan.Click
        SetActiveTool("PanTool")
    End Sub

    Private Sub BezierToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemBezier.Click
        SetActiveTool("BezierTool")
    End Sub
    Private Sub LineToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemLine.Click
        SetActiveTool("LineTool")
    End Sub

    Private Sub RectangleToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemRectangle.Click
        SetActiveTool("RectangleTool")
    End Sub

    Private Sub SetActiveTool(ByVal toolActive As Tool)
        Me.diagramComponent.Controller.ActiveTool = toolActive
    End Sub

    Private Sub SetActiveTool(ByVal toolName As String)
        diagramComponent.Controller.ActivateTool(toolName)
    End Sub
    Private Sub EllipseToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemEllipse.Click
        SetActiveTool("EllipseTool")
    End Sub

    Private Sub SelectToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemSelect.Click
        SetActiveTool("SelectTool")
    End Sub

    Private Sub PolygonToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemPolygon.Click
        SetActiveTool("PolygonTool")
    End Sub

    Private Sub PencilToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemPencil.Click
        SetActiveTool("PencilTool")
    End Sub

    Private Sub PolylineToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemPolyline.Click
        SetActiveTool("PolyLineTool")
    End Sub

    Private Sub TextToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemText.Click
        SetActiveTool("TextTool")
    End Sub

    Private Sub ArcToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemSpline.Click
        SetActiveTool("SplineTool")
    End Sub

    Private Sub RoundedRectangleToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemRoundRect.Click
        SetActiveTool("RoundRectTool")
    End Sub

    Private Sub CurveToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemCurve.Click
        SetActiveTool("CurveTool")
    End Sub

    Private Sub ClosedCurveToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemClosedCurve.Click
        SetActiveTool("ClosedCurveTool")
    End Sub
    Private Sub RichTextToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemRichText.Click
        SetActiveTool("RichTextTool")
    End Sub

    Private Sub ZoomToolActivate(ByVal sender As Object, ByVal e As EventArgs) Handles barItemZoom.Click
        SetActiveTool("ZoomTool")
    End Sub

    Private Sub FormatChanged(ByVal sender As Object, ByVal e As EventArgs)
        UpdateUITextFormatting()
    End Sub

    <EventHandlerPriority(True)> _
    Private Sub View_PropertyChanged(ByVal evtArgs As Syncfusion.Windows.Forms.Diagram.PropertyChangedEventArgs)
        If evtArgs.PropertyName = DPN.Magnification Then
            Me.comboBoxBarItemMagnification.TextBoxValue = Me.Diagram.View.Magnification.ToString() & "%"
        End If
    End Sub

    Private Sub mnuLayout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuLayout.Click
        Dim dialog As LayoutDialog = New LayoutDialog(Me.Diagram)
        dialog.Show()
    End Sub
    Private Sub mnuFlipBoth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuFlipBoth.Click
        Me.Diagram.FlipHorizontal()
        Me.Diagram.FlipVertical()
    End Sub

    Private Sub Application_Idle(ByVal sender As Object, ByVal e As EventArgs)
        Exit Sub
        If Not Me.Diagram.Controller Is Nothing Then
            Dim count As Integer = Me.Diagram.Controller.SelectionList.Count
            ' update context menu
            mnuAlgnLeft.Enabled = (count >= 2)
            mnuAlgnCenter.Enabled = (count >= 2)
            mnuAlgnRight.Enabled = (count >= 2)
            mnuAlgnTop.Enabled = (count >= 2)
            mnuAlgnMiddle.Enabled = (count >= 2)
            mnuAlgnBottom.Enabled = (count >= 2)
            mnuFlipHoriz.Enabled = (count > 0)
            mnuFlipVert.Enabled = (count > 0)
            mnuFlipBoth.Enabled = (count > 0)
            mnuGGroup.Enabled = (count > 1)
            mnuGUngroup.Enabled = True
            mnuOrdBTF.Enabled = (count > 0)
            mnuOrdBF.Enabled = (count > 0)
            mnuOrdSB.Enabled = (count > 0)
            mnuOrdSTB.Enabled = (count > 0)
            mnuRtClockwise.Enabled = (count > 0)
            mnuRtCClockwise.Enabled = (count > 0)
            mnuRsSameWidth.Enabled = (count > 1)
            mnuRsSameHeight.Enabled = (count > 1)
            mnuRsSameSize.Enabled = (count > 1)
            mnuRsSpaseAcross.Enabled = (count > 1)
            mnuRsSpaceDown.Enabled = (count > 1)
        End If
    End Sub


#Region "Layout bar"
    Private Sub barItemSpaceAcross_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRsSpaseAcross.Click, barItemSpaceAcross.Click
        Me.diagramComponent.SpaceAcross()
    End Sub
    Private Sub barItemSpaceDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRsSpaceDown.Click, barItemSpaceDown.Click
        Me.diagramComponent.SpaceDown()
    End Sub
    Private Sub barItemSameWidth_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRsSameWidth.Click, barItemSameWidth.Click
        Me.diagramComponent.SameWidth()
    End Sub
    Private Sub barItemSameHeight_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRsSameHeight.Click, barItemSameHeight.Click
        Me.diagramComponent.SameHeight()
    End Sub
    Private Sub barItemSameSize_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuRsSameSize.Click, barItemSameSize.Click
        Me.diagramComponent.SameSize()
    End Sub
#End Region

#Region "Align bar"
    Private Sub barItemAlignLeft_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlgnLeft.Click, barItemAlignLeft.Click
        Me.diagramComponent.AlignLeft()
    End Sub
    Private Sub barItemAlignCenter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlgnCenter.Click, barItemAlignCenter.Click
        Me.diagramComponent.AlignCenter()
    End Sub
    Private Sub barItemAlignRight_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlgnRight.Click, barItemAlignRight.Click
        Me.diagramComponent.AlignRight()
    End Sub
    Private Sub barItemAlignTop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlgnTop.Click, barItemAlignTop.Click
        Me.diagramComponent.AlignTop()
    End Sub
    Private Sub barItemAlignMiddle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlgnMiddle.Click, barItemAlignMiddle.Click
        Me.diagramComponent.AlignMiddle()
    End Sub
    Private Sub barItemAlignBottom_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAlgnBottom.Click, barItemAlignBottom.Click
        Me.diagramComponent.AlignBottom()
    End Sub
#End Region

#End Region

#Region "fields"
    Private m_hashControllerTools As Hashtable
    Private m_BaritemActiveTool As BarItem
#End Region

#Region "properties"
    Private Property Tools() As Hashtable
        Get
            If m_hashControllerTools Is Nothing Then
                m_hashControllerTools = New Hashtable()
            End If

            Return m_hashControllerTools
        End Get
        Set(ByVal value As Hashtable)
            If Not m_hashControllerTools Is value Then
                m_hashControllerTools = value
            End If
        End Set
    End Property
    Private Property ActiveToolBarItem() As BarItem
        Get
            Return m_BaritemActiveTool
        End Get
        Set(ByVal value As BarItem)
            If Not m_BaritemActiveTool Is value Then
                If Not m_BaritemActiveTool Is Nothing Then
                    ' Uncheck
                    m_BaritemActiveTool.Checked = False
                End If

                ' Update baritem
                m_BaritemActiveTool = value


                If Not diagramComponent.Controller.TextEditor.IsEditing Then
                    diagramComponent.Focus()
                End If

                If Not m_BaritemActiveTool Is Nothing Then
                    ' Check new baritem
                    m_BaritemActiveTool.Checked = True
                End If
            End If
        End Set
    End Property
#End Region

    Public Sub Entrada(ByRef pDTC As DTCDataContext, ByRef pPropuesta_Plano As Propuesta_Plano)
        Try
            '  Syncfusion.Windows.Forms.LocalizationProvider.Provider = New LocalizationDemo.Localizer

            'LocalizationProvider.Provider = New Localizer()
            Me.AplicarDisseny()

            oLinqPropuestaPlano = pPropuesta_Plano
            oDTC = pDTC

            Dim horizontalspacing As Single = 15
            Dim verticalspacing As Single = 15
            horizontalspacing = MeasureUnitsConverter.Convert(horizontalspacing, MeasureUnits.Pixel, MeasureUnits.Millimeter)
            verticalspacing = MeasureUnitsConverter.Convert(verticalspacing, MeasureUnits.Pixel, MeasureUnits.Millimeter)
            Diagram.View.Grid.HorizontalSpacing = horizontalspacing
            Diagram.View.Grid.VerticalSpacing = verticalspacing

            Me.Diagram.View.PageBorderStyle.MeasureUnit = MeasureUnits.Millimeter
            Me.Diagram.View.Grid.MeasureUnit = MeasureUnits.Millimeter
            Me.Diagram.Model.MeasurementUnits = MeasureUnits.Millimeter
            Me.Diagram.View.Grid.HorizontalSpacing = 5
            Me.Diagram.View.Grid.VerticalSpacing = 5
            Me.Diagram.View.Grid.DashOffset = 0
            Me.Diagram.View.Grid.GridStyle = GridStyle.Line


            Me.Diagram.ShowRulers = True
            Me.Diagram.Model.LineStyle.LineColor = Color.Transparent


            If IsNothing(pPropuesta_Plano.ID_PlanoBinario) = False Then
                Dim dStream As MemoryStream = New MemoryStream(oLinqPropuestaPlano.PlanoBinario.Fichero.ToArray, 0, oLinqPropuestaPlano.PlanoBinario.Fichero.Length)
                dStream.Position = 0
                Diagram.LoadBinary(dStream)
                dStream.Close()
                dStream.Dispose()
                dStream = Nothing
            Else
                Me.Diagram.Model.LogicalSize = New SizeF(210, 297)
                Me.Diagram.Model.DocumentScale = New PageScale("A4", 1, MeasureUnits.Millimeter, 1, MeasureUnits.Millimeter)
                Me.Diagram.Model.DocumentSize = New PageSize("A4", 210, MeasureUnits.Millimeter, 297, MeasureUnits.Millimeter)
            End If

            AddHandler Diagram.EventSink.NodeCollectionChanged, AddressOf AlCanviarEstatDelNode
            AddHandler Diagram.EventSink.NodeMouseEnter, AddressOf AlEntrarDinsDelNode
            AddHandler Diagram.EventSink.NodeMouseLeave, AddressOf AlSortirDelNode

            Me.ToolForm.Top = 6
            Me.ToolForm.Left = 156

            Call CargarComboFamilias()

            If oLinqPropuestaPlano.Propuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Pendiente And oLinqPropuestaPlano.Propuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Aprobado Then
                Me.Diagram.AllowDrop = False
                Me.Diagram.DefaultContextMenuEnabled = False

                'Me.Diagram.Enabled = False
                Me.GRD_Linea.Enabled = False
                Me.barGeneral.Items("Guardar").Enabled = False
                Me.barGeneral.Items("GuardarComo").Enabled = False

                Dim _Node As Syncfusion.Windows.Forms.Diagram.Node
                For Each _Node In Me.Diagram.Model.Nodes
                    _Node.EditStyle.Enabled = False
                Next

            End If

            If oLinqPropuestaPlano.Propuesta.SeInstalo = True Then
                Me.L_NotaInformativa.Visible = True
            Else
                Me.L_NotaInformativa.Visible = False
            End If

            Dim thisType As Type
            thisType = Me.GetType()
            Dim st As System.IO.Stream = thisType.Module.Assembly.GetManifestResourceStream(thisType, "basicshapes.edp")
            Me.paletteGroupView1.LoadPalette(Util.ConvertirStreamToByteArray(st))
            st.Close()
            st.Dispose()
            st = Nothing

            Dim _GroupBarItem3 As New Syncfusion.Windows.Forms.Tools.GroupBarItem
            _GroupBarItem3.Text = "Incendios"
            Me.paletteGroupBar1.GroupBarItems.Add(_GroupBarItem3)

            _PaletaGroupView2.LoadPalette("C:\PGM\Abidos\Extres\Diagram\Incendios.edp")
            _GroupBarItem3.Client = _PaletaGroupView2


            '  Me.paletteGroupView1.BackgroundImage = Image.FromFile("..\..\..\..\..\..\..\..\..\Common\images\Diagram\background_2.jpg")
            Me.paletteGroupView1.FlatLook = True
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub barGuardar_Click(sender As System.Object, e As System.EventArgs) Handles barGuardar.Click
        Try
            Dim dStream As MemoryStream = New MemoryStream()
            Me.Diagram.SaveBinary(dStream)

            ' Me.Diagram.Model.MeasurementUnits = MeasureUnits.Pixel 'ho passem a pixels pq si no no ho exporta correctament
            Dim imgformat As ImageFormat = ImageFormat.Jpeg
            Dim img As Image = Me.Diagram.ExportDiagramAsImage(False)
            ' Me.Diagram.Model.MeasurementUnits = MeasureUnits.Millimeter

            If IsNothing(oLinqPropuestaPlano.ID_PlanoBinario) = True Then
                Dim pepe As New PlanoBinario
                pepe.Fichero = dStream.ToArray
                pepe.Foto = Util.ImageToBinary(img).ToArray
                oDTC.PlanoBinario.InsertOnSubmit(pepe)
                oDTC.SubmitChanges()
                ' oLinqPropuestaPlano.PlanoBinario.Foto = Util.ImageToBinary(img).ToArray
                oLinqPropuestaPlano.PlanoBinario = pepe
                oDTC.SubmitChanges()
                pepe = Nothing
            Else

                If oLinqPropuestaPlano.Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente Or oLinqPropuestaPlano.Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado Then
                    oLinqPropuestaPlano.PlanoBinario.Fichero = dStream.ToArray
                    oLinqPropuestaPlano.PlanoBinario.Foto = Util.ImageToBinary(img).ToArray
                    oDTC.SubmitChanges()
                End If
            End If

                'Dim pepe2 As Drawing.Image = Util.BinaryToImage(oLinqPropuestaPlano.PlanoBinario.Foto.ToArray)
                'img.Save("c:\pgm\pepe.jpg", imgformat)

                'pepe2.Save("c:\pgm\aaa.jpg", imgformat)
                'Util.BinaryToImage(oLinqPropuestaPlano.PlanoBinario.Foto.ToArray).Save("C:\pgm\pepe.bmp")


            'Mensaje.Mostrar_Mensaje("Plano guardado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
            img.Dispose()
            img = Nothing
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub barGuardarComo_Click(sender As System.Object, e As System.EventArgs) Handles barGuardarComo.Click
        Me.saveDiagramDialog.FileName = oLinqPropuestaPlano.Propuesta.Codigo

        If Me.saveDiagramDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Dim strFileName As String = Me.saveDiagramDialog.FileName
            ' search for file extension
            Dim options As System.Text.RegularExpressions.RegexOptions = System.Text.RegularExpressions.RegexOptions.IgnoreCase
            Dim match As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(strFileName, "([^.]*)$", options)
            If match.Success Then
                Dim imgDiagram As Image

                Select Case match.Value
                    Case "svg"
                        Dim tosvg As ToSvg = New ToSvg()
                        ' Get Diagram Nodes Bounding Rectangle.
                        Dim rectBounding As RectangleF = Diagram.Model.GetBoundingRect()
                        Dim gfx As Graphics = tosvg.GetRealGraphics(New Size(CInt(rectBounding.Width), CInt(rectBounding.Height)))
                        Diagram.ExportDiagramToGraphics(gfx)
                        tosvg.Save(strFileName)

                    Case "emf"
                        'Save Diagram to created image.
                        imgDiagram = Diagram.ExportDiagramAsImage(False)

                        ' Save image as metafile.
                        imgDiagram.Save(strFileName, ImageFormat.Emf)
                    Case "png"
                        'Save Diagram to created image.
                        imgDiagram = Diagram.ExportDiagramAsImage(False)

                        ' Save image as metafile.
                        imgDiagram.Save(strFileName, ImageFormat.Png)
                    Case "jpg", "jpeg"
                        'Save Diagram to created image.
                        imgDiagram = Diagram.ExportDiagramAsImage(False)

                        ' Save image as metafile.
                        imgDiagram.Save(strFileName, ImageFormat.Jpeg)
                    Case "tiff"
                        'Save Diagram to created image.
                        imgDiagram = Diagram.ExportDiagramAsImage(False)

                        ' Save image as metafile.
                        imgDiagram.Save(strFileName, ImageFormat.Tiff)
                    Case "gif"
                        'Save Diagram to created image.
                        imgDiagram = Diagram.ExportDiagramAsImage(False)

                        ' Save image as metafile.
                        imgDiagram.Save(strFileName, ImageFormat.Gif)
                    Case "bmp"
                        'Save Diagram to created image.
                        imgDiagram = Diagram.ExportDiagramAsImage(False)

                        ' Save image as metafile.
                        imgDiagram.Save(strFileName, ImageFormat.Bmp)

                End Select
                Mensaje.Mostrar_Mensaje("Plano exportado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
            End If
        End If
    End Sub

    Private Sub barPageSetup_Click(sender As System.Object, e As System.EventArgs) Handles barPageSetup.Click
        Dim activeDiagram As Controls.Diagram = Diagram

        If activeDiagram Is Nothing OrElse activeDiagram.Model Is Nothing Then
            Return
        End If

        Using dlgPageSetup As Syncfusion.Windows.Forms.Diagram.PageSetupDialog = New Syncfusion.Windows.Forms.Diagram.PageSetupDialog(activeDiagram.View)
            'dlgPageSetup.PageScale.DrawingScaleUnit = MeasureUnits.Millimeter
            If dlgPageSetup.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                activeDiagram.UpdateView()
            End If
        End Using
    End Sub

    Private Sub barPrintPreview_Click(sender As System.Object, e As System.EventArgs) Handles barPrintPreview.Click
        Dim activeDiagram_Renamed As Controls.Diagram = Diagram

        If Not activeDiagram_Renamed Is Nothing Then
            Dim printDoc As PrintDocument = activeDiagram_Renamed.CreatePrintDocument()
            Dim printPreviewDlg As PrintPreviewDialog = New PrintPreviewDialog()
            printPreviewDlg.StartPosition = FormStartPosition.CenterScreen

            printDoc.PrinterSettings.FromPage = 0
            printDoc.PrinterSettings.ToPage = 0
            printDoc.PrinterSettings.PrintRange = PrintRange.AllPages

            printPreviewDlg.Document = printDoc
            printPreviewDlg.ShowDialog(Me)
        End If
    End Sub

    Private Sub barImprimir_Click(sender As System.Object, e As System.EventArgs) Handles barImprimir.Click
        Dim activeDiagram_Renamed As Controls.Diagram = Diagram
        If Not activeDiagram_Renamed Is Nothing Then
            Dim printDoc As PrintDocument = activeDiagram_Renamed.CreatePrintDocument()
            Dim printDlg As PrintDialog = New PrintDialog()
            printDlg.Document = printDoc

            printDlg.AllowSomePages = True

            If printDlg.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                printDoc.PrinterSettings = printDlg.PrinterSettings
                printDoc.Print()
            End If
        End If
    End Sub

    Private Sub C_Familia_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Familia.ValueChanged
        If Me.C_Familia.SelectedIndex = -1 Then
            Exit Sub
        End If
        Call CargarGrid_Linies()
    End Sub

    Private Sub GRD_Linea_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Linea.M_GRID_DoubleClickRow
        Dim pRow As UltraGridRow
        pRow = Me.GRD_Linea.GRID.ActiveRow
        Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault

        Dim Identificador As String
        If oLinqPropuestaPlano.Propuesta.SeInstalo = True Then
            Identificador = IIf(_Linea.IdentificadorDelProducto.Length <> 0, _Linea.IdentificadorDelProducto, Nothing)
        Else
            Identificador = _Linea.Identificador
        End If



        Call CrearSymbol(Identificador, _Linea.ID_Propuesta_Linea)
        Call barGuardar_Click(Nothing, Nothing)
    End Sub

    'Private Sub CrearSymbol(ByVal pCaption As String, ByVal pIDPropuesta_Linea As Integer)
    '    Dim Grup As Group = New Group()
    '    Dim units As MeasureUnits = MeasureUnits.Pixel

    '    MsgBox(Me.ActiveToolBarItem.Text)
    '    ' Create and add the outer and inner shape nodes that will make up the Symbol
    '    Dim Symbol As Syncfusion.Windows.Forms.Diagram.FilledPath = Nothing
    '    oTipusObjectes = TipusObjectes.ShadowedBox
    '    Select Case oTipusObjectes
    '        Case TipusObjectes.Rectangle
    '            Symbol = New Syncfusion.Windows.Forms.Diagram.Rectangle(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 40, 25, units)
    '        Case TipusObjectes.Circle
    '            Symbol = New Syncfusion.Windows.Forms.Diagram.Ellipse(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 25, 25, units)
    '        Case TipusObjectes.RondedRectangle
    '            Symbol = New Syncfusion.Windows.Forms.Diagram.RoundRect(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 40, 25, units)
    '        Case TipusObjectes.Square
    '            Symbol = New Syncfusion.Windows.Forms.Diagram.Rectangle(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 25, 25, units)
    '            Dim pepe As ShadowStyle
    '            pepe.Color = Color.Red

    '        Case TipusObjectes.ShadowedBox
    '            Symbol = New Syncfusion.Windows.Forms.Diagram.Rectangle(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 25, 25, units)
    '        Case TipusObjectes.Elipse
    '            Symbol = New Syncfusion.Windows.Forms.Diagram.Ellipse(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 25, 25, units)
    '            ' Case TipusObjectes.Triangle
    '            'Symbol = New Syncfusion.Windows.Forms.Diagram.(MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Width, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, MeasureUnitsConverter.Convert(Me.Diagram.Document.Model.DocumentSize.Height, MeasureUnits.Millimeter, MeasureUnits.Pixel) / 2, 25, 25, units)

    '    End Select


    '    'Symbol.FillStyle.Type = FillStyleType.Hatch
    '    'Symbol.FillStyle.HatchBrushStyle = HatchStyle.Wave
    '    Symbol.Name = "Rectangle"

    '    Symbol.FillStyle.ForeColor = Color.DarkBlue
    '    Symbol.FillStyle.Color = Color.LightSkyBlue
    '    Symbol.ShadowStyle.Visible = False


    '    Grup.AppendChild(Symbol)
    '    ' Add a Label for the Symbol
    '    Dim lbl As Syncfusion.Windows.Forms.Diagram.Label = New Syncfusion.Windows.Forms.Diagram.Label(Grup, IIf(IsNothing(pCaption), "", pCaption))
    '    lbl.Position = Position.Center
    '    lbl.ReadOnly = True
    '    Grup.Labels.Add(lbl)
    '    Grup.DrawPorts = False
    '    Grup.EnableCentralPort = True
    '    Grup.Name = String.Concat(Symbol.Name)
    '    Grup.Tag = pIDPropuesta_Linea
    '    Me.Diagram.Model.AppendChild(Grup)
    'End Sub

    Private Sub CrearSymbol(ByVal pCaption As String, ByVal pIDPropuesta_Linea As Integer)
        Dim _Node As Node
        Select Case Me.paletteGroupBar1.SelectedItem
            Case 0
                _Node = Me.paletteGroupView1.SelectedNode.Clone
            Case 1
                _Node = _PaletaGroupView2.SelectedNode.Clone
        End Select
        '

        Dim pepe As New System.Drawing.SizeF(25, 25)
        _Node.Size = pepe


        Dim Grup As Group = New Group()
        Dim lbl As Syncfusion.Windows.Forms.Diagram.Label = New Syncfusion.Windows.Forms.Diagram.Label(Grup, IIf(IsNothing(pCaption), "", pCaption))
        lbl.Position = Position.BottomCenter
        lbl.ReadOnly = True
        lbl.AdjustRotateAngle = True
        lbl.UpdatePosition = True
        lbl.WrapText = False
        'lbl.InheritContainerMeasureUnits = True
        'lbl.Position = Position.Custom
        ' lbl.SizeToNode = True
        Grup.AppendChild(_Node)
        Grup.Labels.Add(lbl)
        Grup.DrawPorts = False
        Grup.EnableCentralPort = True
        Grup.Name = String.Concat(_Node.Name)
        Grup.Tag = pIDPropuesta_Linea

        Me.Diagram.Model.AppendChild(Grup)
        ' AddHandler Grup.OnPropertyChanging, AddressOf AlFerUnResizeDunObjecte

        Call Me.SelectToolActivate(Nothing, Nothing) 'Aquesta línea fa que si tens un objecte seleccionat es deseleccioni
    End Sub

    Private Sub diagramComponent_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs)
        '        Dim node As Node = CType(Me.Diagram.Controller.GetNodeUnderMouse(New Point(e.X, e.Y)), Node)
    End Sub

    Private Sub AlCanviarEstatDelNode(ByVal e As CollectionExEventArgs)
        If e.ChangeType = CollectionExChangeType.Remove Then
            If IsNothing(e.Element.tag) = False Then
                BD.EjecutarConsulta("Delete top (1) From Propuesta_Plano_ElementosIntroducidos Where ID_Propuesta_Linea=" & CInt(e.Element.tag))
                Call CargarGrid_Linies()
            End If
        End If
        If e.ChangeType = CollectionExChangeType.Insert Then
            If IsNothing(e.Element.tag) = False Then
                BD.EjecutarConsulta("Insert Into Propuesta_Plano_ElementosIntroducidos(ID_Propuesta_Linea, ID_Propuesta_Plano) Values (" & CInt(e.Element.tag) & " , " & oLinqPropuestaPlano.ID_Propuesta_Plano & ")")
                Call CargarGrid_Linies()
            End If
        End If
    End Sub

    Private Sub AlEntrarDinsDelNode(ByVal e As NodeMouseEventArgs)
        If IsNothing(e.Node.Tag) = False Then

            Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Node.Tag)).FirstOrDefault

            toolTipInfo = New Syncfusion.Windows.Forms.Tools.ToolTipInfo
            toolTipInfo.BackColor = SystemColors.Control
            Dim _Emplazamiento As String
            Dim _Planta As String
            Dim _Zona As String
            Dim _Uso As String
            If _Linea.Instalacion_Emplazamiento Is Nothing Then
                _Emplazamiento = ""
            Else
                _Emplazamiento = _Linea.Instalacion_Emplazamiento.Descripcion
            End If
            If _Linea.Instalacion_Emplazamiento_Planta Is Nothing Then
                _Planta = ""
            Else
                _Planta = _Linea.Instalacion_Emplazamiento_Planta.Descripcion
            End If

            If _Linea.Instalacion_Emplazamiento_Zona Is Nothing Then
                _Zona = ""
            Else
                _Zona = _Linea.Instalacion_Emplazamiento_Zona.Descripcion
            End If

            If _Linea.Uso Is Nothing Then
                _Uso = ""
            Else
                _Uso = _Linea.Uso
            End If

            toolTipInfo.Body.Text = "Emplazamiento:" & _Emplazamiento & Constants.vbCrLf & "Planta:" & _Planta & Constants.vbCrLf & "Zona:" & _Zona & Constants.vbCrLf & "Uso:" & _Uso
            toolTipInfo.Header.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (CByte(0)))
            toolTipInfo.Header.Text = _Linea.Producto.Codigo & "  " & _Linea.Descripcion
            toolTipInfo.Header.TextAlign = ContentAlignment.MiddleCenter
            superToolTip1 = New Syncfusion.Windows.Forms.Tools.SuperToolTip
            'Dim pepe As New Syncfusion.Windows.Forms.Tools.SuperToolTip
            superToolTip1.Show(Me.toolTipInfo, New Point(MousePosition.X, MousePosition.Y), 3000)
            'Me.superToolTip1.Show(Me.toolTipInfo, New Point(MousePosition.X, MousePosition.Y), 3000)
        End If
    End Sub

    Private Sub AlSortirDelNode(ByVal e As NodeMouseEventArgs)
        Me.superToolTip1.Hide()
    End Sub


    Private Sub CargarComboFamilias()
        Util.Cargar_Combo(C_Familia, "Select ID_Producto_Familia, Familia From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & oLinqPropuestaPlano.Propuesta.ID_Propuesta & " and ID_Producto_Familia_Simbolo<>0   " & Filtres() & " Group By ID_Producto_Familia, Familia Order by Familia", True)
    End Sub

    Private Sub CargarGrid_Linies()
        Me.GRD_Linea.M.clsUltraGrid.Cargar("Select *, Unidad - (Select COUNT(*) From Propuesta_Plano_ElementosIntroducidos as A Where A.ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and A.ID_Propuesta_Plano=" & oLinqPropuestaPlano.ID_Propuesta_Plano & ") as CantidadDisponible From C_Propuesta_Linea Where  Activo=1 " & Filtres() & " and ID_Propuesta=" & oLinqPropuestaPlano.Propuesta.ID_Propuesta & " and ID_Producto_Familia=" & Me.C_Familia.Value & " And Unidad>(Select COUNT(*) From Propuesta_Plano_ElementosIntroducidos as A Where A.ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and A.ID_Propuesta_Plano=" & oLinqPropuestaPlano.ID_Propuesta_Plano & ")", BD, 1)
    End Sub

    Private Function Filtres() As String
        Dim _SQL As String = ""

        If oLinqPropuestaPlano.Propuesta.SeInstalo = True Then
            _SQL = " and IdentificadorDelProducto is not null  "
        End If

        If IsNothing(oLinqPropuestaPlano.Instalacion_Emplazamiento_Zona) = False Then
            _SQL = _SQL & " and ID_Instalacion_Emplazamiento_Zona=" & oLinqPropuestaPlano.ID_Instalacion_Emplazamiento_Zona
        Else
            If IsNothing(oLinqPropuestaPlano.Instalacion_Emplazamiento_Planta) = False Then
                _SQL = _SQL & " and ID_Instalacion_Emplazamiento_Planta=" & oLinqPropuestaPlano.ID_Instalacion_Emplazamiento_Planta
            Else
                If IsNothing(oLinqPropuestaPlano.Instalacion_Emplazamiento) = False Then
                    _SQL = _SQL & " and ID_Instalacion_Emplazamiento=" & oLinqPropuestaPlano.ID_Instalacion_Emplazamiento
                End If
            End If
        End If
        Return _SQL
    End Function

    Private Sub GRD_Linea_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Linea.M_Grid_InitializeRow
        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Linea.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

    Private Sub paletteGroupView1_GroupViewItemSelected(sender As Object, e As EventArgs) Handles paletteGroupView1.GroupViewItemSelected

    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.Diagram.BackgroundImage = Nothing
        '  RemoveHandler CType(diagramComponent.EventSink, DiagramViewerEventSink).ToolActivated, AddressOf DiagramForm_ToolActivated
        '   RemoveHandler diagramComponent.Controller.TextEditor.FormatChanged, AddressOf FormatChanged
        '  RemoveHandler Application.Idle, AddressOf Application_Idle
        Me.FormTancar()
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overrides Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                ' TODO: eliminar estado administrado (objetos administrados).

                'If oDTC Is Nothing = False Then
                '    'oDTC.Dispose()
                'End If


                _PaletaGroupView2.Dispose()
                document.Dispose()
                childFrameBarManager.Dispose()
                smBarItemImages.Dispose()
                barItemSelect.Dispose()
                barItemLine.Dispose()
                barItemRectangle.Dispose()
                barItemEllipse.Dispose()
                barItemText.Dispose()
                barItemPolyline.Dispose()
                barItemPolygon.Dispose()
                barItemSpline.Dispose()
                barItemCurve.Dispose()
                barItemClosedCurve.Dispose()
                barItemImage.Dispose()
                barItemGroup.Dispose()
                barItemUngroup.Dispose()
                barItemBringToFront.Dispose()
                barItemSendToBack.Dispose()
                barItemBringForward.Dispose()
                barItemSendBackward.Dispose()
                barItemNudgeUp.Dispose()
                barItemNudgeDown.Dispose()
                barItemNudgeLeft.Dispose()
                barItemNudgeRight.Dispose()
                barItemRotateLeft.Dispose()
                barItemRotateRight.Dispose()
                barItemFlipVertical.Dispose()
                barItemFlipHorizontal.Dispose()
                barDrawing.Dispose()
                barNode.Dispose()
                barNudge.Dispose()
                barRotate.Dispose()
                barItemPan.Dispose()
                bar1.Dispose()
                components.Dispose()
                barItemShowGrid.Dispose()
                barItemSnapToGrid.Dispose()
                barItemZoom.Dispose()
                comboBoxBarItemMagnification.Dispose()
                barItemOrthogonalLink.Dispose()
                barItemLink.Dispose()
                barLinks.Dispose()
                barItemDirectedLink.Dispose()
                barItemRichText.Dispose()
                barItemRoundRect.Dispose()
                barItemPencil.Dispose()
                barItemBoldText.Dispose()
                barItemAlignTextLeft.Dispose()
                barItemCenterText.Dispose()
                barItemAlignTextRight.Dispose()
                comboBoxBarItemFontFamily.Dispose()
                comboBoxBarItemPointSize.Dispose()
                bar2.Dispose()
                barItemItalicText.Dispose()
                barItemUnderlineText.Dispose()
                barItemTextColor.Dispose()
                bar3.Dispose()
                barItemLoadScript.Dispose()
                barItemRunScript.Dispose()
                barItemEditScript.Dispose()
                barItemStopScript.Dispose()
                barItemSuperscript.Dispose()
                barItemSubscript.Dispose()
                barItemUpper.Dispose()
                barItemLower.Dispose()
                'fileName_Renamed.dispose()
                barItemStrikeoutText.Dispose()
                barItemBezier.Dispose()
                barLayout.Dispose()
                barAlign.Dispose()
                barItemSpaceAcross.Dispose()
                barItemSpaceDown.Dispose()
                barItemSameWidth.Dispose()
                barItemSameHeight.Dispose()
                barItemSameSize.Dispose()
                barItemAlignLeft.Dispose()
                barItemAlignCenter.Dispose()
                barItemAlignRight.Dispose()
                barItemAlignTop.Dispose()
                barItemAlignMiddle.Dispose()
                barItemAlignBottom.Dispose()
                contextMenu1.Dispose()
                mnuAlgn.Dispose()
                mnuFlip.Dispose()
                mnuAlgnLeft.Dispose()
                mnuAlgnCenter.Dispose()
                mnuAlgnRight.Dispose()
                mnuAlgnTop.Dispose()
                mnuAlgnMiddle.Dispose()
                mnuAlgnBottom.Dispose()
                mnuGrouping.Dispose()
                mnuOrder.Dispose()
                mnuRotate.Dispose()
                mnuResize.Dispose()
                mnuFlipHoriz.Dispose()
                mnuFlipVert.Dispose()
                mnuFlipBoth.Dispose()
                mnuGGroup.Dispose()
                mnuOrdBTF.Dispose()
                mnuOrdBF.Dispose()
                mnuOrdSB.Dispose()
                mnuOrdSTB.Dispose()
                mnuLayout.Dispose()
                mnuRtClockwise.Dispose()
                mnuRtCClockwise.Dispose()
                mnuRsSameWidth.Dispose()
                mnuRsSameHeight.Dispose()
                mnuRsSameSize.Dispose()
                mnuRsSpaseAcross.Dispose()
                mnuRsSpaceDown.Dispose()
                mnuGUngroup.Dispose()
                'm_biSelectedAlignment.Dispose()
                superToolTip1.Dispose()
                'penDiagramDialog.dispose()
                saveDiagramDialog.Dispose()
                barGeneral.Dispose()
                barGuardar.Dispose()
                smallImageList.Dispose()
                barGuardarComo.Dispose()
                barPageSetup.Dispose()
                barPrintPreview.Dispose()
                barImprimir.Dispose()
                GRD_Linea.Dispose()
                C_Familia.Dispose()
                UltraLabel3.Dispose()
                UltraPanel1.Dispose()
                SplitContainer1.Dispose()
                paletteGroupBar1.Dispose()
                propertyEditor1.Dispose()
                paletteGroupView1.Dispose()
                GroupBarItem1.Dispose()
                L_NotaInformativa.Dispose()
                'toolTipInfo.dispose
                'Diagram.BackgroundImage = Nothing
                'Diagram.BackgroundImage.Dispose()
                diagramComponent.Model.Dispose()
                diagramComponent.Dispose()

            End If

            ' Me.R_DetallesExtendidos.RichText.Dispose()
            ' Me.R_DetallesExtendidos.Dispose()
            ' Me.R_Observaciones.RichText.Dispose()
            ' Me.R_Observaciones.Dispose()

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
            'RemoveHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDades

            oDTC = Nothing
            superToolTip1 = Nothing
            document = Nothing
            childFrameBarManager = Nothing
            smBarItemImages = Nothing
            contextMenu1 = Nothing
            openDiagramDialog = Nothing
            saveDiagramDialog = Nothing
            smallImageList = Nothing
            diagramComponent = Nothing
            _PaletaGroupView2 = Nothing
            oTipusObjectes = Nothing
            oLinqPropuestaPlano = Nothing
        End If
        Me.disposedValue = True
        MyBase.Dispose(True)
    End Sub
#End Region
End Class
'End Namespace
