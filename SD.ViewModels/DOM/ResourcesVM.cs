using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class ResourcesVM : VMBase
    {
        public ResourcesVM()
        {
            _resources = new ObservableCollection<ResourceBase>();
        }

        private ObservableCollection<ResourceBase> _resources;
        public ObservableCollection<ResourceBase> Resources
        {
            get { return _resources; }
            set
            {
                _resources = value;
                NotifyPropertyChanged("Resources");
            }
        }

        private ResourceBase _selectedResource;
        [JsonIgnore]
        [XmlIgnore]
        public ResourceBase SelectedResource
        {
            get { return _selectedResource; }
            set
            {
                _selectedResource = value;
                NotifyPropertyChanged("SelectedResource");
            }
        }

        private bool resourceNameExist(string name)
        {
            return _resources.Count(r => r.Name == name) > 0;
        }
        public void AddResourceCommand(string name, ResourceBase.Types type, byte[] data)
        {
            if (resourceNameExist(name))
                return;

            switch (type)
            {
                case ResourceBase.Types.Image:
                    var myImageRes = new ImageResource(name, data);
                    _resources.Add(myImageRes);
                    SelectedResource = myImageRes;
                    break;
                case ResourceBase.Types.Video:
                    var myVideoRes = new VideoResource(name, data);
                    _resources.Add(myVideoRes);
                    SelectedResource = myVideoRes;
                    break;
                default:
                    break;
            }
        }
        public void RemoveResourceCommand()
        {
            if (_selectedResource != null)
                _resources.Remove(_selectedResource);
        }
    }
}
