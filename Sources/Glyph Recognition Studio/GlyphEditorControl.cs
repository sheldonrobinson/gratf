﻿// Glyph Recognition Studio
// http://www.aforgenet.com/projects/gratf/
//
// Copyright © Andrew Kirillov, 2010
// andrew.kirillov@aforgenet.com
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using AForge.Vision.GlyphRecognition;

namespace GlyphRecognitionStudio
{
    public partial class GlyphEditorControl : Control
    {
        // some pens and brushes used to paint the control
        private Pen   grayPen = new Pen( Color.DarkGray );
        private Brush whiteBrush = new SolidBrush( Color.White );
        private Brush blackBrush = new SolidBrush( Color.Black );

        // glyph to edit
        private Glyph glyph = null;

        // enable/disable of editing border (false const for now)
        private const bool disableEditingBorders = true;

        public GlyphEditorControl( )
        {
            InitializeComponent( );
            SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.SuspendLayout( );
            // 
            // GlyphEditorControl
            // 
            this.MouseClick += new System.Windows.Forms.MouseEventHandler( this.GlyphEditorControl_MouseClick );
            this.ResumeLayout( false );

        }

        #endregion

        // Dispose the object
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( components != null )
                    components.Dispose( );

                // free graphics resources
                grayPen.Dispose( );
                whiteBrush.Dispose( );
            }
            base.Dispose( disposing );
        }

        // Set glyph to edit
        public void SetGlyph( Glyph glyph )
        {
            this.glyph = glyph;
            Invalidate( );
        }

        // Paint the control
        protected override void OnPaint( PaintEventArgs pe )
        {
            Graphics g = pe.Graphics;
            int clientWidth  = ClientRectangle.Width;
            int clientHeight = ClientRectangle.Height;

            if ( glyph == null )
            {
                g.FillRectangle( whiteBrush, 0, 0, clientWidth - 1, clientHeight - 1 );
                g.DrawRectangle( grayPen, 0, 0, clientWidth - 1, clientHeight - 1 );
            }
            else
            {
                int size = glyph.Size;
                int cellWidth  = clientWidth / size;
                int cellHeight = clientHeight / size;

                // paint each cell
                for ( int i = 0; i < size; i++ )
                {
                    for ( int j = 0; j < size; j++ )
                    {
                        g.FillRectangle( ( glyph.Data[i, j] == 0 ) ? blackBrush : whiteBrush,
                            j * cellWidth, i * cellHeight, cellWidth, cellHeight );
                        g.DrawRectangle( grayPen, j * cellWidth, i * cellHeight, cellWidth - 1, cellHeight - 1 );
                    }
                }
            }
        }

        // Handle mouse click event
        private void GlyphEditorControl_MouseClick( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Left )
            {
                int size = glyph.Size;
                int cellWidth  = ClientRectangle.Width / size;
                int cellHeight = ClientRectangle.Height / size;

                int cellX = e.X / cellWidth;
                int cellY = e.Y / cellHeight;

                if ( ( cellX >= 0 ) && ( cellY >= 0 ) && ( cellX < size ) && ( cellY < size ) )
                {
                    if ( ( disableEditingBorders ) &&
                         ( ( cellX == 0 ) || ( cellY == 0 ) || ( cellX == size - 1 ) || ( cellY == size - 1 ) ) )
                    {
                        System.Media.SystemSounds.Beep.Play( );
                    }
                    else
                    {
                        glyph.Data[cellY, cellX] ^= 1;
                        Invalidate( );
                    }
                }
            }
        }
    }
}
