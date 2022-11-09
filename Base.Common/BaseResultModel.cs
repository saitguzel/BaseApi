namespace Base.Common
{
    public class BaseResultModel
    {
        public BaseResultModel()
        {
            Errors = new List<string>();
            ResultObject = new List<object>();
        }
        public bool Success
        {
            get { return (Errors.Count == 0 && ResultObject.Count > 0); }
        }
        public void AddError(string error)
        {
            Errors.Add(error);
        }
        public void AddResultObject(object resulObject)
        {
            ResultObject.Add(resulObject);
        }
        public List<string> Errors { get; set; }
        public List<object> ResultObject { get; set; }
    }
}
