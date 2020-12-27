using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ZH.ViewModel
{
    class GameViewModel : ViewModelBase
    {
        public ObservableCollection<Field> Fields { get; set; }
        Model _model;
        public DelegateCommand StartGameSmall { get; private set; }
        public DelegateCommand StartGameMedium { get; private set; }
        public DelegateCommand StartGameLarge { get; private set; }
        public int BoardSize { get { return _model.Size; } }

        public string OnTurn { get { return _model.OnTurn == 0 ? "Black" : "Gray"; } set { OnTurn = value; } }
        public int CurrentPlayer { get { return _model.OnTurn == 1 ? _model.Player1Next + 1 : _model.Player2Next + 1;  } }

        public EventHandler<int> StartGame;

        public GameViewModel(Model m)
        {
            _model = m;
            Fields = new ObservableCollection<Field>();
            StartGameSmall = new DelegateCommand(
                (_) => OnStartGame(4)
            );
            StartGameMedium = new DelegateCommand(
                (_) => OnStartGame(6)
            );
            StartGameLarge = new DelegateCommand(
                (_) => OnStartGame(8)
            );
            _model.generateTable += GenerateTable;
        }

        private void OnStartGame(int e)
        {
            if (StartGame != null)
                StartGame(this, e);
        }

        private void GenerateTable(object sender, int e)
        {
            Fields.Clear();
            for (Int32 i = 0; i < e; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < e; j++)
                {
                    Fields.Add(new Field
                    {
                        Text = _model.getMapElem(i, j) == 0 ? String.Empty : _model.getMapElem(i, j) >  4 ? (_model.getMapElem(i, j) - 4).ToString() : (_model.getMapElem(i, j)).ToString(),
                        Color = _model.getMapElem(i, j) == 0 ? "White" : _model.getMapElem(i, j) < 5 ? "Black" : "Gray",
                        X = i,
                        Y = j,
                        Number = i * _model.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
            OnPropertyChanged("OnTurn");
            OnPropertyChanged("CurrentPlayer");
        }

        private void StepGame(int idx)
        {
            Field field = Fields[idx];

            _model.StepGame(field.X, field.Y);
            RefreshTable();
        }

        private void RefreshTable()
        {
            Fields.Clear();
            for (Int32 i = 0; i < _model.Size; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < _model.Size; j++)
                {
                    Fields.Add(new Field
                    {
                        Text = _model.getMapElem(i, j) == 0 ? String.Empty : _model.getMapElem(i, j) > 4 ? (_model.getMapElem(i, j) - 4).ToString() : (_model.getMapElem(i, j)).ToString(),
                        Color = _model.getMapElem(i, j) == 0 ? "White" : _model.getMapElem(i, j) < 5 ? "Black" : "Gray",
                        X = i,
                        Y = j,
                        Number = i * _model.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
            OnPropertyChanged("OnTurn");
            OnPropertyChanged("CurrentPlayer");
        }

    }
}
