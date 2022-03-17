using System;
using System.Collections.Generic;

namespace Banks
{
    public class Client
    {
        private List<AbstractAccount> _accounts = new List<AbstractAccount>();
        private string _name;
        private string _surname;
        private string _address;
        private int _passportNumber = -1;
        private bool _flagNotification = false;

        public Client()
        {
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void SetSurname(string surname)
        {
            _surname = surname;
        }

        public void SetAddress(string address)
        {
            _address = address;
            CheckNotRequireFields();
        }

        public void SetPassportNumber(int number)
        {
            _passportNumber = number;
            CheckNotRequireFields();
        }

        public string GetName()
        {
            return _name;
        }

        public string GetSurname()
        {
            return _surname;
        }

        public string GetAddress()
        {
            return _address;
        }

        public int GetPassportNumber()
        {
            return _passportNumber;
        }

        public bool CheckNotRequireFields()
        {
            if (GetAddress() != " " && GetPassportNumber() != -1)
            {
                return true;
            }

            return false;
        }

        public void RemoveAccount(AbstractAccount account)
        {
            int i = _accounts.IndexOf(account);
            if (i < 0)
            {
                throw new Exception("This account doesn't exist");
            }

            _accounts.RemoveAt(i);
        }

        public List<AbstractAccount> GetAccounts()
        {
            return _accounts;
        }

        public void AddAccount(AbstractAccount account)
        {
            _accounts.Add(account);
        }

        public void UpdateInformationAboutMoneyInAccounts(DateTime newDate)
        {
            foreach (AbstractAccount account in _accounts)
            {
                account.UpdateAmount(newDate);
            }
        }

        public void NotifyChangesConditions()
        {
            _flagNotification = true;
        }

        public bool GetFlagNotification()
        {
            return _flagNotification;
        }

        public string GetPersonalInformation()
        {
            return GetName() + GetSurname() + GetAddress() + GetPassportNumber();
        }

        public List<string> GetInformationAccounts()
        {
            var accountsInformation = new List<string>();
            foreach (AbstractAccount account in _accounts)
            {
                accountsInformation.Add(account.GetInformation());
            }

            return accountsInformation;
        }

        public void ChangeConditionInBank(BankData data)
        {
            foreach (AbstractAccount account in _accounts)
            {
                account.UpdateInformation(data);
            }
        }
    }
}