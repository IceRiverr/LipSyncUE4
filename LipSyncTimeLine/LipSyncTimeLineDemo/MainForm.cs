﻿using LipSyncTimeLineControl;
using LipSyncTimeLineControl.Enums;
using LipSyncTimeLineControl.Events;
using LipSyncTimeLineControl.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace LipSyncTimeLineDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            foreach (MorphTimelineTrack phonemeTemplate in MorphTemplate.MorphTemplateDictionary.Values.Where(x => x.TimelineTrackType == TimelineTrackTypeEnum.Phoneme))
                morphListBoxPhoneme.Items.Add(phonemeTemplate);

            foreach (MorphTimelineTrack expressionTemplate in MorphTemplate.MorphTemplateDictionary.Values.Where(x => x.TimelineTrackType == TimelineTrackTypeEnum.Expression))
                morphListBoxExpression.Items.Add(expressionTemplate);

            timeline.SelectionChanged += TimelineSelectionChanged;
            timeline.SoundPlayerTick += SoundPlayerTick;
        }

        private static void TimelineSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (null != selectionChangedEventArgs.Deselected)
            {
                foreach (TimelineTrackBase track in selectionChangedEventArgs.Deselected)
                {
                    Debug.WriteLine("Deselected: " + track);
                }
            }

            if (null != selectionChangedEventArgs.Selected)
            {
                foreach (TimelineTrackBase track in selectionChangedEventArgs.Selected)
                {
                    Debug.WriteLine("Selected: " + track);
                }
            }
        }

        private void SoundPlayerTick(object sender, SoundPlayerTickEventsArgs e)
        {
            Debug.WriteLine($"Elapsed Time: {e.ElapsedTime}");

            TimelineTrackBase track = timeline.GetCurrentPhonemeFromElapsedTime(e.ElapsedTime / 10);

            pbPhonemeImage.Image = track != null ? track.Bitmap : LipSyncTimeLineControl.Properties.Resources.Phoneme_None;
        }

        private void morphListBoxPhoneme_MouseDown(object sender, MouseEventArgs e)
        {
            if (morphListBoxPhoneme.Items.Count == 0)
                return;

            int index = morphListBoxPhoneme.IndexFromPoint(e.X, e.Y);
            if (index > -1)
                DoDragDrop(morphListBoxPhoneme.Items[index], DragDropEffects.All);
        }

        private void morphListBoxExpression_MouseDown(object sender, MouseEventArgs e)
        {
            if (morphListBoxExpression.Items.Count == 0)
                return;

            int index = morphListBoxExpression.IndexFromPoint(e.X, e.Y);
            if (index > -1)
                DoDragDrop(morphListBoxExpression.Items[index], DragDropEffects.Copy);
        }

        private void btnSaveProject_Click(object sender, EventArgs e)
        {
            timeline.SaveProject();
        }

        private void btnLoadProject_Click(object sender, EventArgs e)
        {
            timeline.LoadProject();
        }

        private void btnAddSubtitleTrack_Click(object sender, EventArgs e)
        {
            timeline.AddSubtitleTrack();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            timeline.Export();
        }

        private void btnAddWordsTrack_Click(object sender, EventArgs e)
        {
            timeline.AddWordsTrack(rtbPhraseText.Text);
        }

        private void btnGeneratePhoneme_Click(object sender, EventArgs e)
        {
            timeline.GeneratePhoneme();
        }
    }
}
