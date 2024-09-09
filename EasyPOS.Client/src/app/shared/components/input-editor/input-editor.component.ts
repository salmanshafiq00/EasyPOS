import { Component, EventEmitter, forwardRef, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';

@Component({
  selector: 'app-input-editor',
  templateUrl: './input-editor.component.html',
  styleUrls: ['./input-editor.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputEditorComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => InputEditorComponent),
      multi: true
    }
  ]
})
export class InputEditorComponent implements ControlValueAccessor, Validator, OnInit {
  @Input() disabled: boolean = false;
  @Input() label: string = '';
  @Input() hidden: boolean = false;
  @Input() inputId: string = '';
  @Input() required: boolean = false;
  // @Input() initialValue: string = '';
  @Input() inline: boolean = false;
  @Input() plugins: string = '';
  @Input() toolbar: string = '';
  @Input() outputFormat: string = 'html';
  @Input() maxLength: number = 500;
  @Input() height: number = 300;
  @Input() width: number = null;
  @Input() resize: boolean = true;
  @Input() autoresize: boolean = false;
  @Input() min_height: number = 200;
  @Input() max_height: number = 500;
  @Input() min_width: number = 200;
  @Input() max_width: number = 500;
  @Output() content: EventEmitter<string> = new EventEmitter<string>();

  init: any = {};
  apiKey = 'pjk2c1pab50c3jmwcaeahfup499fqtua0avtqrj9dnfbs7c4';


  defaultPlugins: string = 'anchor autolink charmap insertdatetime preview codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker tinymcespellchecker permanentpen powerpaste advtable advcode editimage advtemplate ai mentions tinycomments tableofcontents footnotes mergetags autocorrect typography fullscreen inlinecss markdown autoresize';
  defaultToolbar: string = 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | align lineheight | checklist numlist bullist indent outdent | removeformat | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | emoticons charmap';

  value: string = '';
  isInvalid: boolean = false;
  onTouched: () => void = () => { };
  onChangeFn: (value: any) => void = () => { };

  ngOnInit() {
    this.init = {
      height: this.height,
      resize: this.resize,
      plugins: this.plugins || this.defaultPlugins,
      toolbar: this.toolbar || this.defaultToolbar,
      inline: this.inline,
      setup: (editor: any) => {
        editor.on('change keyup', (event: any) => {
          this.value = editor.getContent({ format: this.outputFormat });
          let content = editor.getContent({ format: 'text' });
          this.content.emit(content);
          this.onChangeFn(this.value);
        });
        editor.on('blur', () => {
          this.onTouched();
        });
      },
      tinycomments_mode: 'embedded',
      tinycomments_author: 'Salman',
      ai_request: (request, respondWith) => respondWith.string(() => Promise.reject("See docs to implement AI Assistant")),
    };
  }

  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    if (this.required && !control.value) {
      this.isInvalid = true;
      return { required: true };
    }
    this.isInvalid = false;
    return null;
  }
}
