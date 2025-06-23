import { Component, inject, OnInit } from '@angular/core';
import { NgIf } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngxs/store';
import { filter, from, switchMap, take } from 'rxjs';
import { SharedModule } from '../../../shared/shared.module';
import { SettingsState } from '../../state/settings.state';
import { Settings } from '../../state/settings.action';
import GetAll = Settings.GetAll;
import SaveMinimumWage = Settings.SaveMinimumWage;

@Component({
	selector: 'app-settings',
	standalone: true,
	imports: [NgIf, SharedModule],
	templateUrl: './settings.component.html',
	styleUrl: './settings.component.scss'
})
export class SettingsComponent implements OnInit {
	private store = inject(Store);

	form!: FormGroup;
	settings$ = this.store.select(SettingsState.getSettings);

	constructor(private fb: FormBuilder) {}

	ngOnInit(): void {
		const dispatch$ = from(this.store.dispatch(new GetAll()));

		this.form = this.fb.group({
			// Hardcoded for now, because there is only one setting currently in the system
			minimumWage: [null, [Validators.required, Validators.min(1)]]
		});

		dispatch$
			.pipe(
				switchMap(() => this.settings$),
				filter((settings) => settings.length > 0),
				take(1)
			)
			.subscribe((settings) => {
				const financialSetting = settings.filter((x) => x.key === 'MinimumWage')[0];
				console.warn(financialSetting);
				if (financialSetting) {
					this.form.patchValue({
						minimumWage: financialSetting.value
					});
				}
			});
	}

	saveSettings(): void {
		if (this.form.valid) {
			const { minimumWage } = this.form.value;

			this.store.dispatch(new SaveMinimumWage(minimumWage));
		} else {
			this.form.markAllAsTouched();
		}
	}
}
