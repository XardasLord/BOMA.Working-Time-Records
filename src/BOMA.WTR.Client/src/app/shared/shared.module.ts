import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppMaterialModule } from './modules/app-material.module';
import { ToastrModule } from 'ngx-toastr';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@NgModule({
	declarations: [],
	imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, AppMaterialModule, ToastrModule.forRoot()],
	exports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, AppMaterialModule, ToastrModule],
	providers: []
})
export class SharedModule {}
