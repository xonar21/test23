Vue.component('Listing', {
	template: `
		<div :id="'listing-' + id" :class="className" class="mb-4" v-if="items.length > 0">
			<div class="w-100">
				<table class="columns w-100">
					<thead>
						<tr>
							<th v-for="(col, i) in columns" :key="i">
								{{ col.name }}
							</th>
						</tr>
					</thead>
					<tbody>
						<tr v-for="item in items" :key="item.id">
							<td v-for="(col,i) in columns" :key="i">								
								<template >
									{{ item[withoutPrefix(col.id)] }}
								</template>
							</td>
							<td style="width: 20px">
								<a href="javascript:void(0)"><span class="icon-delete" @click.stop.prevent="deleteItem(item.id)"></span></a>
							</td>
							<td style="width: 20px">
								<a href="javascript:void(0)"><span class="icon-edit" @click.stop.prevent="editItem(item.id)"></span></a>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
	`,
	props: {
		id: String,
		className: String,
		items: Array,
		columns: Array,
		inputPrefix: String,
		deleteItem: Function,
		editItem: Function
	},
	methods: {
		withoutPrefix(column) {
			return column.replace(this.inputPrefix, '');
		},
		isFieldSlot(fieldName) {
			return typeof this.$scopedSlots[fieldName] !== 'undefined'
		},
	/*<template v-if="isFieldSlot(withoutPrefix(col.id))">
								<slot :name="withoutPrefix(col.id)" :row-data="item"></slot>
							</template>*/
	}
});