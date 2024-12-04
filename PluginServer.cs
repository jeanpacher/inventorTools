/////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved
// Written by Forge Partner Development
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
/////////////////////////////////////////////////////////////////////

using Inventor;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;


namespace Bosch_ImportData
{
    [Guid("ca494c79-b928-476b-9f2b-39cffff5c45d")]
    public class PluginServer : ApplicationAddInServer
    {
        public string tabName = "Bosch";
        public string panelName = "DataImport";

        ButtonDefinition BotaoImportData;



        public static Inventor.Application invApp;
        public static InventorServer inventorServer;
        public string inventorID;


        //private Form1 mControlForm;
        // private Inventor.ApplicationEvents m_ApplicationEvents;


        public dynamic Automation { get; private set; }

        public void Activate(ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            Trace.TraceInformation(": Bosch_ImportData (" + Assembly.GetExecutingAssembly().GetName().Version.ToString(4) + "): initializing... ");

            // Initialize AddIn members.
            //inventorServer = addInSiteObject.InventorServer;

            invApp = addInSiteObject.Application;
            Parametros._invApp = addInSiteObject.Application;


            ////m_ApplicationEvents = invApp.ApplicationEvents;
            ////m_ApplicationEvents.OnOpenDocument += this.m_ApplicationEvents_OnOpenDocument;

            //mControlForm = new Form1(invApp);
            //mControlForm.ShowModeless();

            GuidAttribute guidAtt = (GuidAttribute)System.Attribute.GetCustomAttribute(typeof(PluginServer), typeof(GuidAttribute));
            inventorID = "{" + guidAtt.Value + "}";


            // ------------------------------------ AQUI COMEÇA A EDIÇÃO -------------------------------------------------------- //

            #region Criação da Interface (Aba e Painel)
            // PEGANDO OS CONTROLES DO INVENTOR
            ControlDefinitions controles = (ControlDefinitions)invApp.CommandManager.ControlDefinitions;

            // PEGANDO OS CONTROLES DA INTERCACE
            UserInterfaceManager gerenciadorUI = (UserInterfaceManager)invApp.UserInterfaceManager;

            //PEGANDO AS RIBBONS POR AMBIENTE
            Ribbon ribbonPart = gerenciadorUI.Ribbons["Part"];
            Ribbon ribbonAssembly = gerenciadorUI.Ribbons["Assembly"];
            Ribbon ribbonDrawing = gerenciadorUI.Ribbons["Drawing"];
            Ribbon ribbonZeroDoc = gerenciadorUI.Ribbons["ZeroDoc"]; // Acessa a ribbon quando não há documentos abertos

            //CRIANDO UMA NOVA ABA
            RibbonTab TabPart = ribbonPart.RibbonTabs.Add(tabName, "Autodesk.TabPart." + tabName, inventorID);
            RibbonTab TabAssembly = ribbonAssembly.RibbonTabs.Add(tabName, "Autodesk.TabAssembly." + tabName, inventorID);
            RibbonTab TabDrawing = ribbonDrawing.RibbonTabs.Add(tabName, "Autodesk.TabDrawing." + tabName, inventorID);
            RibbonTab TabZero = ribbonZeroDoc.RibbonTabs.Add(tabName, "Autodesk.TabZero." + tabName, inventorID);



            //CRIANDO UM NOVO PAINEL
            RibbonPanel painelPart = TabPart.RibbonPanels.Add(panelName, "Autodesk.PannelPart." + panelName, inventorID);
            RibbonPanel painelAssembly = TabAssembly.RibbonPanels.Add(panelName, "Autodesk.PannelAssembly." + panelName, inventorID);
            RibbonPanel painelDrawing = TabDrawing.RibbonPanels.Add(panelName, "Autodesk.PanelDrawing." + panelName, inventorID);
            RibbonPanel painelZero = TabZero.RibbonPanels.Add(panelName, "Autodesk.PanelZero." + panelName, inventorID);


            #endregion


            //Criação do botão EXPORTAR ARQUIVOS EM LOTE
            IPictureDisp iconeBotaoImportar32x32 = (IPictureDisp)AxHostConverter.ImagemParaPictureDisp(global::Bosch_ImportData.Properties.Resources.Botao_Importar_arquivos_32x32);
            IPictureDisp iconeBotaoImportar16x16 = (IPictureDisp)AxHostConverter.ImagemParaPictureDisp(global::Bosch_ImportData.Properties.Resources.Botao_Importar_arquivos_16x16);


            BotaoImportData = controles.AddButtonDefinition("Importar Arquivos", "Autodesk.KeepCAD.Inventor:BotãoImportarArquivos",
                CommandTypesEnum.kQueryOnlyCmdType, inventorID, "Comando para importar aquivos", "",
                iconeBotaoImportar32x32, iconeBotaoImportar16x16, ButtonDisplayEnum.kAlwaysDisplayText);

            //painelPart.CommandControls.AddButton(BotaoImportData, true);
            //painelAssembly.CommandControls.AddButton(BotaoImportData, true);
            //painelDrawing.CommandControls.AddButton(BotaoImportData, true);
            painelZero.CommandControls.AddButton(BotaoImportData, true);

            BotaoImportData.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(CarregarUI_ImportData);




        }

        public void Deactivate()
        {
            Trace.TraceInformation(": BoschAutomation: deactivating... ");

            // Release objects.
            //invApp = null;
            // Marshal.ReleaseComObject(invApp);


            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void CarregarUI_ImportData(NameValueMap Content)
        {
            ImportFiles ImportData = new ImportFiles();
            ShowFormControl.ShowModelessForm(ImportData);
        }

        //public void m_ApplicationEvents_OnOpenDocument(_Document DocumentObject, string FullDocumentName, EventTimingEnum BeforeOrAfter, NameValueMap Context, out HandlingCodeEnum HandlingCode)
        //{
        //    System.Windows.Forms.MessageBox.Show("OnOpenDocument fires!");

        //    HandlingCode = HandlingCodeEnum.kEventHandled;
        //}


        public void ExecuteCommand(int CommandID)
        {
            // obsolete
        }




    }
    public class AxHostConverter : AxHost
    {
        private AxHostConverter() : base("")
        {
        }

        static public stdole.IPictureDisp ImagemParaPictureDisp(Image image)
        {
            return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
        }

        static public Image PictureDispParaImagem(stdole.IPictureDisp pictureDisp)
        {
            return GetPictureFromIPicture(pictureDisp);
        }
    }

    public class ShowFormControl
    {
        /// <summary>
        ///     Get no Inventor Aplication
        /// </summary>
        public ShowFormControl()
        {
            //InvApp.cadApp = (Application)Marshal.GetActiveObject("Inventor.Application");
        }

        public static bool IsFormOpened(String formName)
        {
            foreach (Form objForm in System.Windows.Forms.Application.OpenForms)
            {
                if (objForm.GetType().Name == formName)
                {
                    objForm.WindowState = FormWindowState.Normal;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Mostra o Form em modo Modal
        /// </summary>
        /// <param name="modalCmdDlg"></param>
        public static void ShowModalForm(Form modalCmdDlg)
        {
            WindowsWrapperForForm window =
                new WindowsWrapperForForm(
                    (IntPtr)PluginServer.invApp.MainFrameHWND);
            modalCmdDlg.Activate();
            modalCmdDlg.ShowInTaskbar = false;
            //ShowDialog is used..for Modal forms
            modalCmdDlg.ShowDialog(window);
        }

        /// <summary>
        ///     Mostra o Form em modo Modeless
        /// </summary>
        /// <param name="modelessCmdDlg"></param>
        public static void ShowModelessForm(Form modelessCmdDlg)
        {
            if (!(IsFormOpened(modelessCmdDlg.Name)))
            {
                WindowsWrapperForForm window = new WindowsWrapperForForm((IntPtr)PluginServer.invApp.MainFrameHWND);
                modelessCmdDlg.Activate();
                modelessCmdDlg.ShowInTaskbar = false;
                modelessCmdDlg.StartPosition = FormStartPosition.CenterScreen;
                modelessCmdDlg.Show(window);
            }
        }

        /// <summary>
        ///     Classe WindowsWrapper para enquadramento do form em modeless
        /// </summary>
        private class WindowsWrapperForForm : IWin32Window
        {
            private IntPtr _mHwnd;
            private int _p;

            public WindowsWrapperForForm(IntPtr handle)
            {
                _mHwnd = handle;
            }

            #region IWin32Window Members

            public IntPtr Handle
            {
                get { return _mHwnd; }
            }

            #endregion
        }
    }



}
