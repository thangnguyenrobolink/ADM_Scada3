using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.PlcService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using System.Windows.Forms;

namespace ADM_Scada.Modules.Plc.ViewModels
{
    public class DashBoardViewModel : BindableBase
    {
        private IPLCCommunicationService pLCCommunicationService;
        private ObservableCollection<VariableModel> variables;
        public ObservableCollection<VariableModel> Variables { get => variables; set => SetProperty(ref variables, value); }
        public DelegateCommand IncCommand { get;  set; }
        public DelegateCommand ImageBrowseCommand { get; private set; }
        private void ImageBrowse()
        {

            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp|All files|*.*",
                InitialDirectory = "\\"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> ImagePathList = new List<string>
                {
                    openFileDialog1.FileName
                };
               // ImagePath = ImagePathList[0];
            }
        }
        public DashBoardViewModel(IPLCCommunicationService _pLCCommunicationService)
        {
            pLCCommunicationService = _pLCCommunicationService;
            Variables = pLCCommunicationService.GetAllVariables();
            IncCommand = new DelegateCommand(Action);
            ImageBrowseCommand = new DelegateCommand(ImageBrowse);
        }
        public void Action()
        {
            Variables[0].Value = Variables[0].Value + 1;
            Variables = Variables;
        }
    }
} 