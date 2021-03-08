using System;
using System.Collections.Concurrent;

namespace EFurni.Presentation.Extensions.ButtonController
{
    public class ButtonState
    {
        private string state1;
        private string state2;

        public ButtonState(string firstState, string secondState)
        {
            state1 = firstState;
            state2 = secondState;
        }

        public string Get()
        {
            return state1;
        }

        public void ToggleState()
        {
            string f1 = state1;
            string f2 = state2;

            state1 = f2;
            state2 = f1;
        }
    }

    public class ButtonStateStorage
    {
        private ConcurrentDictionary<string, ButtonState> settingMap = new ConcurrentDictionary<string, ButtonState>();

        public string Get(string setting)
        {
            if (!settingMap.ContainsKey(setting))
                return string.Empty;

            return settingMap[setting].Get();
        }

        public void Trigger(string setting)
        {
            settingMap[setting].ToggleState();
        }

        public void TriggerAll()
        {
            foreach (var setting in settingMap.Keys)
            {
                settingMap[setting].ToggleState();
            }
        }

        public void Add(string name, string state1, string state2)
        {
            settingMap.TryAdd(name, new ButtonState(state1, state2));
        }

        public void Clear()
        {
            settingMap.Clear();
        }
    }

    public class ButtonController
    {
        private ConcurrentDictionary<string, ButtonStateStorage> _buttonStates =
            new ConcurrentDictionary<string, ButtonStateStorage>();

        private string _lastTriggeredButton;
        private bool radioStyl;
        private bool _nodoubeClick;
        
        public ButtonController(bool radioStyle = true,bool noDoubleClick = false)
        {
            radioStyl = radioStyle;
            _nodoubeClick = noDoubleClick;
        }

        public ButtonStateStorage this[string buttonName] => _buttonStates[buttonName];

        public void SetButtonStates(string settingName, string firstState, string secondState)
        {
            _buttonStates[settingName].Add(settingName, firstState, secondState);
        }

        public void RegisterButton(string name)
        {
            _buttonStates.TryAdd(name, new ButtonStateStorage());
        }

        public bool ButtonExist(string name)
        {
            return _buttonStates.TryGetValue(name, out _);
        }

        public void TriggerButton(string name)
        {
            if (!ButtonExist(name))
                return;

            if (name == _lastTriggeredButton)
            {
                if(_nodoubeClick)
                    return;
                
                _buttonStates[_lastTriggeredButton].TriggerAll();
                _lastTriggeredButton = null;
                return;
            }

            if (radioStyl && !string.IsNullOrEmpty(_lastTriggeredButton))
            {
                _buttonStates[_lastTriggeredButton].TriggerAll();
            }

            _lastTriggeredButton = name;
            _buttonStates[name].TriggerAll();
        }

        public void Clear()
        {
            foreach (var buttonState in _buttonStates.Values)
            {
                buttonState.Clear();
            }

            _buttonStates.Clear();
        }
    }
}