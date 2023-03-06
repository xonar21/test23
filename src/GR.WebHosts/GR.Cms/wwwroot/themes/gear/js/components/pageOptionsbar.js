Vue.component('PageOptionsBar', {
	template: `
		<div v-bind:class="[className ? className : addDefaultClassName && 'page-options-bar py-2 bg-white']">
			<div class="d-flex align-items-center flex-wrap">
				<button v-if="collapsable"
						class="collapse-main mr-10px"
						type="button"
						data-toggle="collapse"
						aria-expanded="false"
						@click="hasGetDataListener && !dataLoaded && $emit('getData')"
						:data-target="'#' + collapsableTargedId"
						:aria-controls="collapsableTargedId">
					<i data-feather="chevron-down"
					   width="14"
					   height="14"
					   class="collapse-main__icon"></i>
				</button>
				<h6 v-if="pageOptionsName"
					class="m-0">
					{{pageOptionsName}}
				</h6>
                <PageFilters ref="filters"
							 v-if="filters"
							 :filters="filters"
							 :groupBy="groupBy"
							 @filterPageResult="filterPageResult"
							 @groupResult="groupResult"/>
                <SelectFilters ref="modifiedFilters"
							   v-if="selectFilters || inputFilters"
							   :selectFilters="selectFilters"
							   :inputFilters="inputFilters"
							   @filterGrid="filterGrid" />
				<div class="flex-grow-1">
					<div class="page-options d-flex"
						 v-if="haveOptions">
						<component
							v-for="(option, index) in pageOptionsComponents"
							:key="index"
							:is="option.name"
							v-bind="option.props">
						</component>
					</div>
				</div>
				<div v-if="searcheable" class="divider"></div>
				<div v-if="searcheable" class="w-auto">
					<input
						type="text"
						class="form-control search-input w-md-100-i mt-md-0 mt-2"
						:placeholder= "searchPlaceholder"
						v-model="searchInput"
					>
				</div>
			</div>
		</div>
	`,
	data() {
		return {
			searchString: ''
		}
	},
	props: {
		searcheable: Boolean,
		pageOptionsComponents: Array,
		pageId: String,
		filters: Array,
		modifiedFilters: Array,
		groupBy: Array,
		className: String,
		pageOptionsName: String,
		collapsable: Boolean,
		collapsableTarged: String,
		addDefaultClassName: { type: Boolean, default: true },
		dataLoaded: { type: Boolean, default: false }
	},
	mounted() {
		feather.replace();
	},
	computed: {
		selectFilters() {
			return this.modifiedFilters?.filter(val => val.type === 'select');
		},
		inputFilters() {
			return this.modifiedFilters?.filter(val => val.type === 'input')
		},
		hasGetDataListener() {
			return this.$listeners && this.$listeners.getData
		},
		haveOptions() {
			if (this.pageOptionsComponents) {
				return this.pageOptionsComponents.length > 0;
			} else {
				return false;
			}
		},
		searchInput: {
			get() {
				return this.searchString;
			},
			set(val) {
				this.$emit('searchInput', val);
			}
		},
		collapsableTargedId() {
			return this.collapsableTarged;
		},
		searchPlaceholder() {
			return t('system_search');
		}
	},
	methods: {
		filterPageResult(val) {
			this.$emit('filterResult', val);
		},
		groupResult(val) {
			this.$emit('groupResult', val);
		},
		filterGrid(val) {
			this.$emit('modifiedFiltersResult', val);
		}
	}
});

Vue.component('SelectFilters', {
	template: `
		<div class="d-flex select-filters mr-10px flex-wrap mod-select">
			<select class="selectpicker"
					v-for="(selectFilter, i) in selectFilters"
					:data-actions-box="selectFilter.actionsBox"
					:data-live-search="selectFilter.searchable"
					:multiple="selectFilter.multiple"
					:name="selectFilter.name">
                <option disabled selected value="" v-if="selectFilter.placeholder">{{ selectFilter.placeholder }}</option>
				<option v-for="(option, i) in selectFilter.optionList"
						:key="i"
						:value="option.value">
					{{ option.label }}
				</option>
			</select>
			<input v-for="(inputFilter, i) in inputFilters"
				   :placeholder="inputFilter.placeholder"
				   :name="inputFilter.name"
				   class="datepicker" />
		</div>
	`,
	data() {
		return {
			form: {
				selectValues: [],
				inputValues: []
			},
		}
	},
	props: {
		selectFilters: Array,
		inputFilters: Array
	},
	mounted() {
		const self = this;
		$(".select-filters .datepicker").datepicker()
			.on("changeDate", (e) => {
				this.setInputValues(e);
			});
		$(".select-filters .selectpicker").selectpicker()
			.on("changed.bs.select", function (e, clickedIndex, newValue, oldValue) {
				self.setSelectValues($(this).attr('name'), $(this).val());
			});
	},
	updated() {
		$('.selectpicker').selectpicker('refresh');
	},
	methods: {
		setSelectValues(selectName, value) {
			const selectValues = this.form.selectValues;
			const foundValue = selectValues.length && selectValues.find(obj => obj?.id === selectName);
			if (!foundValue) {
				this.form.selectValues.push({
					id: selectName,
					value
				});
			} else {
				foundValue.id = selectName;
				foundValue.value = value
			}
			this.emitValues();
		},
		setInputValues(e) {
			const inputValues = this.form.inputValues;
			const foundValue = inputValues.length && inputValues.find(obj => obj?.id === e.delegateTarget.name);
			if (!foundValue) {
				this.form.inputValues.push({
					id: e.delegateTarget.name,
					value: moment(e.date).format("DD/MM/YYYY")
				});
			} else {
				foundValue.id = e.delegateTarget.name;
				foundValue.value = moment(e.date).format("DD/MM/YYYY");
			}
			this.emitValues();
		},
		emitValues() {
			this.$emit('filterGrid', this.form)
		}
	}
});

Vue.component('PageFilters', {
	template: `
		<div class="d-flex">
            <div class="dropdown" v-click-outside="hideDropdown">
                <button type="button" class="btn btn-primary btn-square-32" @click="toggleDropdown">
                    <span class="filter-icon"></span>
                </button>
                <div v-show="dropdownVisible" class="dropdown-menu dropdown-menu-left d-standart-block" >
                    <div class="d-flex filters-groupBy-group">
						<div v-if="groupBy" class="page-groupBy">
							<PageGroupByFields ref="groupBy" :fields="groupBy" @emitValue="groupByValue" />
						</div>
						<div class="page-filter" v-for="filter in filterValues">
							<PageFilterField :filterData="filter" @emitValue="filterValue" :key="filter.id + filter.key"/>
						</div>
					</div>
                </div>
            </div>
            <div class="filter-wrapper d-md-flex d-none flex-wrap" style="margin-top: -4px;">
                <span v-for="(tag, i) in filtersTags" class="badge badge-outline-primary active-filter-item ml-1 mt-1" :key="i">
                    {{ tag.label }}
                    <span @click="resetFilter(tag.filter, tag.value)" class="x-icon cursor-pointer"></span>
                </span>
            </div>
        </div>
	`,
	data() {
		return {
			filterValues: [],
			filterValuesResult: [],
			filtersTags: [],
			dateFormat: 'DD.MM.YYYY',
			filterKey: 0,
			dropdownVisible: false
		}
	},
	props: {
		groupBy: Array,
		filters: Array
	},
	created() {
		this.filterValues = this.filters.map((x, i) => {
			x.key = `${x.id}-${i}`;
			return x;
		});
		this.filterValuesResult = this.filterValues.map((x) => x);
		this.setFilterTags();
	},
	methods: {
		hideDropdown() {
			this.dropdownVisible = false;
		},
		toggleDropdown() {
			this.dropdownVisible = !this.dropdownVisible;
		},
		filterValue(value, filterId, filterType) {
			if (filterType === 'dateRange') {
				const matchedFilter = this.filterValues.find(f => f.id === filterId);
				const startFilter = this.filterValuesResult.find(f => f.id === matchedFilter.idStartDate);
				const endFilter = this.filterValuesResult.find(f => f.id === matchedFilter.idEndDate);
				const addDateFilter = (filter, prop, id) => {
					if (moment(value[prop], this.dateFormat).isValid()) {
						if (filter) {
							filter.values[0].active = true;
							filter.values[0].value = value[prop];
							filter.values[0].label = value[prop];
						} else {
							this.filterValuesResult.push({
								id: matchedFilter[id],
								parentId: matchedFilter.id,
								label: matchedFilter.label,
								key: `${matchedFilter[id]}-${this.filterValuesResult.length}`,
								isDate: true,
								values: [{
									active: true,
									value: value[prop],
									label: value[prop]
								}]
							});
						}
					} else {
						if (filter) {
							this.filterValuesResult = this.filterValuesResult.filter(f => f.id !== filter.id);
						}
					}
				}
				addDateFilter(startFilter, 'start', 'idStartDate');
				addDateFilter(endFilter, 'end', 'idEndDate');
			} else {
				const changeFilter = this.filterValuesResult.find(f => f.id === filterId);
				for (let i = 0; i < changeFilter.values.length; i++) {
					if (Array.isArray(value)) {
						changeFilter.values[i].active = value.includes(changeFilter.values[i].value);
					} else if (changeFilter.radio) {
						changeFilter.values[i].active = value === changeFilter.values[i].value;
					} else {
						changeFilter.values[i].value = value;
					}
				}
			}
			this.setFilterTags();
			this.emitValues();
		},
		emitValues() {
			this.$emit('filterPageResult', this.filterValuesResult);
		},
		groupByValue(val) {
			this.$emit('groupResult', val);
		},
		resetFilter(filterId, valueId) {
			const filterToChange = this.filterValuesResult.find(f => f.id === filterId);
			if (filterToChange.isDate) {
				this.filterValuesResult = this.filterValuesResult.filter(f => f.parentId != filterToChange.parentId);
				const parentFilter = this.filterValuesResult.find(f => f.id === filterToChange.parentId);
				const keys = parentFilter.key.split("-");
				parentFilter.key = `${keys[0]}-${parseInt(keys[1]) + 1}`;
			} else {
				const valueTochange = filterToChange.values.find(fv => fv.value === valueId);
				valueTochange.active = false;
				const keys = filterToChange.key.split("-");
				filterToChange.key = `${keys[0]}-${parseInt(keys[1]) + 1}`;
			}
			this.setFilterTags();
			this.emitValues();
		},
		setFilterTags() {
			let array = [];
			this.filterValuesResult.forEach(frv => {
				if (!frv.isDate) {
					frv.values.forEach(v => {
						if (v.active) {
							array.push({
								label: v.label,
								filter: frv.id,
								value: v.value
							});
						}
					});
				}
			});
			const dateFilters = this.filterValuesResult.filter(f => f.isDate);
			if (dateFilters.length > 0) {
				const parentFilterId = dateFilters[0].parentId;
				const parentFilter = this.filterValuesResult.find(f => f.id == parentFilterId);
				const startDateFilter = dateFilters.find(f => f.id === parentFilter.idStartDate);
				const endDateFilter = dateFilters.find(f => f.id === parentFilter.idEndDate);
				let labelStart = '';
				if (startDateFilter) {
					labelStart = moment(startDateFilter.values[0].value, this.dateFormat).isValid ? startDateFilter.values[0].value : '';
				}
				let labelEnd = '';
				if (endDateFilter) {
					labelEnd = moment(endDateFilter.values[0].value, this.dateFormat).isValid ? endDateFilter.values[0].value : '';
				}
				let label = `${labelStart}${labelEnd}`;
				if (labelStart && labelEnd) {
					label = `${labelStart}-${labelEnd}`;
				}
				array.push({
					label: label,
					filter: startDateFilter ? startDateFilter.id : endDateFilter ? endDateFilter.id : null,
					value: ''
				});
			}
			this.filtersTags = array;
		}
	}
});

Vue.component('PageFilterField', {
	template: `
		<div>
			<h6 class="filter-label">{{ filterData.label }}</h6>
			<template v-if="filterData.radio">
				<div v-for="fItem in filterData.values" :key="fItem.value">
					<div class="custom-control custom-radio">
						<input type="radio" class="custom-control-input" :value="fItem.value" :name="'radio-' + filterData.id" :id="filterData.id + '-' + fItem.value" v-model="inputValue"/>
						<label class="custom-control-label" :for="filterData.id + '-' + fItem.value">{{ fItem.label }}</label>
					</div>
				</div>
			</template>
			<template v-else-if="filterData.isDateRange">
				<div class="filter-date">
					<label :for="filterData.id + 'start'" class="cursor-pointer m-0 d-flex h-auto">
						<i data-feather="calendar" color="#A8B0B8" height="22" width="22"></i>
					</label>
					<input
						type="text"
						class="datetimepicker cursor-pointer"
						:id="filterData.id + 'start'"
						v-model="dateRange.start"
					/>
					<a v-show="isValidStartDate" style="height: 15px;" @click.prevent="resetStartDate" href="#">
						<i data-feather="x" color="#FF2850" height="15" width="15"></i>
					</a>
				</div>
				<div class="filter-date">
					<label :for="filterData.id + 'end'" class="cursor-pointer m-0 d-flex h-auto">
						<i data-feather="calendar" color="#A8B0B8" height="22" width="22"></i>
					</label>
					<input
						type="text"
						class="datetimepicker cursor-pointer"
						:id="filterData.id + 'end'"
						v-model="dateRange.end"
					/>
					<a v-show="isValidEndDate" style="height: 15px;" @click.prevent="resetEndDate" href="#">
						<i data-feather="x" color="#FF2850" height="15" width="15"></i>
					</a>
				</div>
			</template>
			<template v-else-if="filterData.values.length > 15">
				<div class="filter-picker">
					<select
						class="selectpicker"
						:id="filterData.id"
						v-model="inputValue"
						data-none-selected-text="None selected"
						data-size="10"
						data-live-search="true"
						multiple="multiple"
						data-width="100%"
						data-virtual-scroll="1500"
					>
						<option
							v-for="(option, index) in filterData.values"
							:key="index"
							:value="option.value"
						>{{ option.label }}</option>
					</select>
				</div>
			</template>
			<template v-else>
				<div v-for="fItem in filterData.values" :key="fItem.value">
					<div class="custom-control custom-checkbox">
						<input type="checkbox" class="custom-control-input" :value="fItem.value" :id="filterData.id + '-' + fItem.value" v-model="inputValue"/>
						<label class="custom-control-label" :for="filterData.id + '-' + fItem.value">{{ fItem.label }}</label>
					</div>
				</div>
			</template>
		</div>
	`,
	data() {
		return {
			dateRange: {
				start: 'Start Date',
				end: 'End Date'
			},
			datepickerFormat: 'MM/DD/YYYY',
			datepickerDisplayFormat: 'DD.MM.YYYY',
			dataInputValue: [],
		}
	},
	props: {
		filterData: Object,
	},
	computed: {
		inputValue: {
			get() {
				return this.dataInputValue;
			},
			set(val) {
				this.dataInputValue = val;
				this.$emit('emitValue', val, this.filterData.id, 'check');
			}
		},
		isValidStartDate() {
			return this.isDate(this.dateRange.start);
		},
		isValidEndDate() {
			return this.isDate(this.dateRange.end);
		}
	},
	mounted() {
		let newArray = [];
		this.filterData.values.forEach(fv => {
			if (fv.active) {
				newArray.push(fv.value);
			}
		});
		this.dataInputValue = newArray;

		feather.replace();
		$(`#${this.id}`).selectpicker();

		const dateConfig = {
			autoclose: true,
			todayHighlight: true
		}

		const setStartDate = e => {
			const startDateMoment = moment(e.date);
			const datePickerStartDate = startDateMoment.format(this.datepickerFormat);
			const endDateMoment = moment(this.dateRange.end, this.datepickerDisplayFormat);
			this.dateRange.start = startDateMoment.format(this.datepickerDisplayFormat);
			if (endDateMoment.isSameOrBefore(startDateMoment)) {
				$(`#${this.filterData.id}end`).datepicker('setStartDate', datePickerStartDate)
					.datepicker('setDate', startDateMoment.format(this.datepickerFormat));
			} else {
				$(`#${this.filterData.id}end`).datepicker('setStartDate', datePickerStartDate);
			}
		}

		$(`#${this.filterData.id}start`).datepicker(dateConfig).on('changeDate', e => {
			setStartDate(e);
		}).on('hide', e => {
			setStartDate(e);
		});

		$(`#${this.filterData.id}end`).datepicker(dateConfig).on('changeDate', e => {
			this.dateRange.end = moment(e.date).format(this.datepickerDisplayFormat);
		}).on('hide', e => {
			const startDateEndPicker = $(`#${this.filterData.id}end`).datepicker('getStartDate');
			if (startDateEndPicker !== Number.NEGATIVE_INFINITY) {
				if (moment(e.date).isAfter(moment(startDateEndPicker))) {
					this.dateRange.end = moment(e.date).format(this.datepickerDisplayFormat);
				} else {
					this.dateRange.end = moment(startDateEndPicker).format(this.datepickerDisplayFormat);
				}
			} else {
				this.dateRange.end = moment(e.date).format(this.datepickerDisplayFormat);
			}
		});
	},
	updated() {
		$(`#${this.filterData.id}`).selectpicker('refresh');
	},
	methods: {
		isDate(string) {
			return moment(string, this.datepickerDisplayFormat).isValid();
		},
		resetStartDate() {
			this.dateRange.start = 'Start Date';
			this.$emit('emitValue', this.dateRange, this.filterData.id, 'dateRange');
		},
		showFilter(e) {
			$(`#${this.filterData.id}`).selectpicker('show');
		},
		resetEndDate() {
			this.dateRange.end = 'End Date';
			this.$emit('emitValue', this.dateRange, this.filterData.id, 'dateRange');
		}
	},
	watch: {
		dateRange: {
			deep: true,
			handler(newVal) {
				this.$emit('emitValue', newVal, this.filterData.id, 'dateRange');
			}
		}
	}
});

Vue.component('PageGroupByFields', {
	template: `
		<div>
			<h6>GroupBy</h6>
			<template v-for="field in fieldsList">
				<div class="custom-control custom-checkbox" @click.stop>
					<input type="checkbox" class="custom-control-input" :value="field.id" :id="field.id + '-groupBy'" v-model="inputValue"/>
					<label class="custom-control-label" :for="field.id + '-groupBy'">{{ field.label }}</label>
				</div>
			</template>
		</div>	
	`,
	data() {
		return {
			fieldsList: [],
			dataInputValue: []
		}
	},
	computed: {
		inputValue: {
			get() {
				return this.dataInputValue;
			},
			set(val) {
				this.dataInputValue = val;
				this.$emit('emitValue', val);
			}
		}
	},
	props: {
		fields: Array
	},
	created() {
		this.fieldsList = [...this.fields];
	}
});