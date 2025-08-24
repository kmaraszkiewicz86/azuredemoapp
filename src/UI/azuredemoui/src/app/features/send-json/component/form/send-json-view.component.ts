import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JsonModel } from '../../send-json.models';
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';


@Component({
  selector: 'app-send-json-view',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './send-json-view.component.html',
  styleUrl: './send-json-view.component.scss'
})
export class SendJsonViewComponent {
  @Input() initialData?: JsonModel;

  @Output() formSubmit = new EventEmitter<JsonModel>();

  form: FormGroup;

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      id: ['', Validators.required],
      name: ['', Validators.required],
      surname: ['', Validators.required],
      age: [null, [Validators.required, Validators.min(0)]],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  ngOnInit() {
    if (this.initialData) {
      this.form.patchValue(this.initialData);
    }
  }

  onSubmit() {
    if (this.form.valid) {
      this.formSubmit.emit(this.form.value);
    }
  }
}
