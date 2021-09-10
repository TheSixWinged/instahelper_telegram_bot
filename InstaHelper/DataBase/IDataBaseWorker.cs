namespace InstaHelper.DataBase
{
    public interface IDataBaseWorker
    {
        public string GetState(long id);
        public void ChangeState(long id, string state);
        public void InputParser(long id, string input);
        public string GetParser(long id);
        public void SetCompetUnique(long id, bool set);
        public bool GetCompetUnique(long id);
        public void SetCompetAuth(long id, bool set);
        public bool GetCompetAuth(long id);
        public void InputCompetUserLogin(long id, string login);
        public string GetCompetUserLogin(long id);
        public void InputCompetPostlink(long id, string postlink);
        public string GetCompetPostlink(long id);
    }
}
