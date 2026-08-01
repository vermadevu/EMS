import { Component, effect, inject, input, output } from '@angular/core';

import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { Department } from '../../../core/models/department';
import { CreateDepartment } from '../../../core/models/create-department';

@Component({
  selector: 'app-department-form',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './department-form.html',
  styleUrl: './department-form.css'
})
export class DepartmentFormComponent {
  readonly loading = input(false);
  readonly department = input<Department | null>(null);
  readonly save = output<CreateDepartment>();
  readonly cancel = output<void>();
  private readonly fb = inject(FormBuilder);

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    description: ['']
  });

  constructor() {
    effect(() => {
      const department = this.department();
      if (!department) {
        this.form.reset({
          name: '',
          description: ''
        });
        return;
      }
      this.form.patchValue({
        name: department.name,
        description: department.description ?? ''
      });
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.save.emit(this.form.getRawValue());

  }
  cancelClick(): void {
    this.cancel.emit();
  }
}