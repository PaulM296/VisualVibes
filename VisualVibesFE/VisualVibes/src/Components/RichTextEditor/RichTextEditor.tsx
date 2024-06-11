import React, { useState, useRef, useEffect } from 'react';
import { Box, IconButton, ClickAwayListener, Popper } from '@mui/material';
import { AiOutlineBold, AiOutlineItalic, AiOutlineUnderline } from 'react-icons/ai';
import EmojiSmile from '@mui/icons-material/EmojiEmotionsOutlined';
import Picker from '@emoji-mart/react';
import data from '@emoji-mart/data';
import './RichTextEditor.css';

type EmojiType = {
  native: string;
};

type RichTextEditorProps = {
  content: string;
  setContent: (content: string) => void;
};

const RichTextEditor: React.FC<RichTextEditorProps> = ({ content, setContent }) => {
  const [activeCommand, setActiveCommand] = useState({ bold: false, italic: false, underline: false });
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = !!anchorEl;
  const editorRef = useRef<HTMLDivElement>(null);

  const toggleEmojiShowing = (event: React.MouseEvent<HTMLButtonElement>) => {
    if (editorRef.current) {
      editorRef.current.focus();
    }
    event.stopPropagation();
    setAnchorEl(anchorEl ? null : event.currentTarget);
  };

  const emojiSelectClick = (emoji: EmojiType) => {
    document.execCommand('insertText', false, emoji.native);
    handleInput();
  };

  const isCommandActive = (command: string) => {
    return document.queryCommandState(command);
  };

  const updateActiveCommand = () => {
    setActiveCommand({
      bold: isCommandActive('bold'),
      italic: isCommandActive('italic'),
      underline: isCommandActive('underline'),
    });
  };

  const execCommand = (command: string) => {
    document.execCommand(command);
    updateActiveCommand();
  };

  useEffect(() => {
    const handleSelectionChange = () => {
      updateActiveCommand();
    };

    document.addEventListener('selectionchange', handleSelectionChange);
    return () => {
      document.removeEventListener('selectionchange', handleSelectionChange);
    };
  }, []);

  const handleInput = () => {
    if (editorRef.current) {
      setContent(editorRef.current.innerHTML);
    }
  };

  useEffect(() => {
    if (editorRef.current && editorRef.current.innerHTML !== content) {
      editorRef.current.innerHTML = content;
    }
  }, [content]);

  return (
    <>
      <div
        ref={editorRef}
        className="editableDiv"
        contentEditable
        onInput={handleInput}
        onFocus={updateActiveCommand}
        style={{ textAlign: 'left' }}
      ></div>
      <Box className="toolbar">
        <IconButton
          size="small"
          onClick={() => execCommand('bold')}
          className={activeCommand.bold ? 'active' : ''}
        >
          <AiOutlineBold />
        </IconButton>
        <IconButton
          size="small"
          onClick={() => execCommand('italic')}
          className={activeCommand.italic ? 'active' : ''}
        >
          <AiOutlineItalic />
        </IconButton>
        <IconButton
          size="small"
          onClick={() => execCommand('underline')}
          className={activeCommand.underline ? 'active' : ''}
        >
          <AiOutlineUnderline />
        </IconButton>
        <ClickAwayListener onClickAway={() => setAnchorEl(null)}>
          <Box className="emojiPickerContainer">
            <IconButton size="small" onClick={toggleEmojiShowing}>
              <EmojiSmile fontSize="small" />
            </IconButton>
            <Popper
              style={{ zIndex: 1400 }}
              id="emoji-panel"
              open={open}
              anchorEl={anchorEl}
              placement="bottom-start"
            >
              <Picker data={data} onEmojiSelect={emojiSelectClick} />
            </Popper>
          </Box>
        </ClickAwayListener>
      </Box>
    </>
  );
};

export default RichTextEditor;
