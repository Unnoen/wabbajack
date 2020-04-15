﻿using System;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using Microsoft.WindowsAPICodePack.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Wabbajack.Lib;

namespace Wabbajack
{
    public class ModlistSettingsEditorVM : ViewModel
    {
        private readonly CompilationModlistSettings _settings;

        [Reactive]
        public string ModListName { get; set; }

        [Reactive]
        public string AuthorText { get; set; }

        [Reactive]
        public string Description { get; set; }

        public FilePickerVM ImagePath { get; }

        [Reactive]
        public string Readme { get; set; }

        [Reactive]
        public string Website { get; set; }

        public IObservable<bool> InError { get; }

        public ModlistSettingsEditorVM(CompilationModlistSettings settings)
        {
            _settings = settings;
            ImagePath = new FilePickerVM()
            {
                ExistCheckOption = FilePickerVM.CheckOptions.IfPathNotEmpty,
                PathType = FilePickerVM.PathTypeOptions.File,
            };
            ImagePath.Filters.Add(new CommonFileDialogFilter("Banner image", "*.png"));

            InError = this.WhenAny(x => x.ImagePath.ErrorState)
                .Select(err => err.Failed)
                .Publish()
                .RefCount();
        }

        public void Init()
        {
            AuthorText = _settings.Author;
            if (!string.IsNullOrWhiteSpace(_settings.ModListName))
            {
                ModListName = _settings.ModListName;
            }
            Description = _settings.Description;
            Readme = _settings.Readme;
            ImagePath.TargetPath = _settings.SplashScreen;
            Website = _settings.Website;
        }

        public void Save()
        {
            _settings.Author = AuthorText;
            _settings.ModListName = ModListName;
            _settings.Description = Description;
            _settings.Readme = Readme;
            _settings.SplashScreen = ImagePath.TargetPath;
            _settings.Website = Website;
        }
    }
}
