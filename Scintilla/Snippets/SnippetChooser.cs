using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ScintillaNet
{
	public partial class SnippetChooser : UserControl
	{
		public SnippetChooser() 
		{
			InitializeComponent();
		}

		private string _snippetList = string.Empty;
		public string SnippetList
		{
			get
			{
				return _snippetList;
			}
			set
			{
				_snippetList = value;
			}
		}

		private Scintilla _scintilla;
		public Scintilla Scintilla
		{
			get
			{
				return _scintilla;
			}
			set
			{
				_scintilla = value;
			}
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLostFocus(e);

			Hide();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			txtSnippet.Text = string.Empty;
			setPosition();

			if (Visible)
			{
				txtSnippet.Focus();
				txtSnippet.AutoComplete.Show(0, _snippetList);
			}
			else
				Scintilla.Focus();
		}

		protected override void OnCreateControl()
		{
			setPosition();
			base.OnCreateControl();
			
			txtSnippet.Focus();
			txtSnippet.AutoComplete.Show(0, _snippetList);
		}
		
		public void setPosition()
		{
			if (!Visible)
				return;
			
			int pos = Scintilla.Caret.Position;
			int x = Scintilla.PointXFromPosition(pos);
			int y = Scintilla.PointYFromPosition(pos);

			this.Location = new Point(x, y);
		}


		private void SnippetChooser_Load(object sender, EventArgs e)
		{
			//	This Scintilla has a very limited command set. Its necessary becuase
			//	Scintilla's AutoComplete system is very sensitive when it comes to
			//	dismissing the window, almost anything will do it and there's really
			//	no practical way to prevent it.
			txtSnippet.Commands.RemoveAllBindings();

			txtSnippet.Commands.AddBinding(Keys.Delete, Keys.None, BindableCommand.Clear);
			txtSnippet.Commands.AddBinding(Keys.Back, Keys.None, BindableCommand.DeleteBack);
			txtSnippet.Commands.AddBinding('Z', Keys.Control, BindableCommand.Undo);
			txtSnippet.Commands.AddBinding('Y', Keys.Control, BindableCommand.Redo);
			txtSnippet.Commands.AddBinding('X', Keys.Control, BindableCommand.Cut);
			txtSnippet.Commands.AddBinding('C', Keys.Control, BindableCommand.Copy);
			txtSnippet.Commands.AddBinding('V', Keys.Control, BindableCommand.Paste);
			txtSnippet.Commands.AddBinding('A', Keys.Control, BindableCommand.SelectAll);

			txtSnippet.Commands.AddBinding(Keys.Down, Keys.None, BindableCommand.LineDown);
			txtSnippet.Commands.AddBinding(Keys.Up, Keys.None, BindableCommand.LineUp);

		}

		private void txtSnippet_KeyDown(object sender, KeyEventArgs e)
		{
			//	The built in Scintilla Command Bindings for left and right
			//	will automatically dismiss the AutoComplete Window, which
			//	we don't want. So instead we have to fake our own left and
			//	right functions
			switch (e.KeyCode)
			{
				case Keys.Right:
					txtSnippet.Caret.Goto(txtSnippet.Caret.Position + 1);
					break;
				case Keys.Left:
					txtSnippet.Caret.Goto(txtSnippet.Caret.Position - 1);
					break;
				case Keys.Enter:
				case Keys.Tab:
					if (txtSnippet.AutoComplete.SelectedIndex >= 0)
						txtSnippet.AutoComplete.Accept();					
					break;
				case Keys.Escape:
					Hide();
					break;
			}				
		}


		private void txtSnippet_AutoCompleteAccepted(object sender, AutoCompleteAcceptedEventArgs e)
		{
			string shortcut = txtSnippet.AutoComplete.SelectedText;
			Hide();
			Scintilla.Snippets.InsertSnippet(shortcut);
		}

		private void txtSnippet_DocumentChange(object sender, NativeScintillaEventArgs e)
		{
			////	If for any reason the window DOES manage to hide itself
			////	we merely reshow it.
			if (!txtSnippet.AutoComplete.IsActive && Visible)
			{
				int pos = Scintilla.Caret.Position;
				Scintilla.Caret.Goto(0);
				txtSnippet.AutoComplete.Show(0, _snippetList);
				Scintilla.Caret.Goto(pos);
			}
		}
	}
}
