<template>
    <div id="home">
        <div class="container">
            <b-loading :is-full-page="true" :active.sync="isLoading" :can-cancel="false"/>

            <div class="columns is-centered is-mobile is-multiline has-side-margin has-negative-top-margin">
                <div class="column is-three-quarters-tablet is-full-desktop is-full-fullhd">
                    <div class="box has-shadow">
                        <div>
                            <b-field grouped>
                                <b-field :label="$t('home.search.name')" position="is-centered">
                                    <b-input type="text" maxlength="30" :placeholder="$t('home.search.name-info')" v-model="searchName" autoComplete="new-password"/>
                                </b-field>

                                <b-field :label="$t('home.search.gender')" position="is-centered" expanded>
                                    <b-autocomplete ref="genderRef" v-model="searchGenderText" :placeholder="$t('home.search.gender-info')" 
                                        :open-on-focus="openOnFocusGender" :keep-first="true" :custom-formatter="dataFormatter()"
                                        :data="filteredGenders" @select="option => searchGender = option" :clearable="true"
                                        dropdown-position="bottom" autoComplete="new-password">

                                        <template slot-scope="props">
                                            <p>
                                                <span class="has-text-semibold">{{ props.option.id }}</span>
                                                <span> - </span>
                                                <span>{{ props.option.name }}</span>
                                            </p>
                                        </template>

                                        <template slot="empty">{{ $t('home.search.no-result') }} {{ searchGenderText }}</template>
                                    </b-autocomplete>
                                </b-field>

                                <b-field :label="$t('home.search.region')" position="is-centered" expanded>
                                    <b-autocomplete ref="regionRef" v-model="searchRegionText" :placeholder="$t('home.search.region-info')" 
                                        :open-on-focus="openOnFocusRegion" :keep-first="true" :custom-formatter="dataFormatter()"
                                        :data="filteredRegions" @select="option => regionSelected(option)" :clearable="true"
                                        dropdown-position="bottom" autoComplete="new-password">

                                        <template slot-scope="props">
                                            <p>
                                                <span class="has-text-semibold">{{ props.option.id }}</span>
                                                <span> - </span>
                                                <span>{{ props.option.name }}</span>
                                            </p>
                                        </template>

                                        <template slot="empty">{{ $t('home.search.no-result') }} {{ searchRegionText }}</template>
                                    </b-autocomplete>
                                </b-field>

                                <b-field :label="$t('home.search.city')" position="is-centered" expanded>
                                    <b-autocomplete ref="cityRef" v-model="searchCityText" :placeholder="$t('home.search.city-info')" 
                                        :open-on-focus="openOnFocusCity" :keep-first="true" :custom-formatter="dataFormatter()"
                                        :data="filteredCities" @select="option => searchCity = option" :clearable="true"
                                        dropdown-position="bottom" autoComplete="new-password">

                                        <template slot-scope="props">
                                            <p>
                                                <span class="has-text-semibold">{{ props.option.id }}</span>
                                                <span> - </span>
                                                <span>{{ props.option.name }}</span>
                                            </p>
                                        </template>

                                        <template slot="empty">{{ $t('home.search.no-result') }} {{ searchCityText }}</template>
                                    </b-autocomplete>
                                </b-field>
                            </b-field>

                            <b-field grouped>                               
                                <b-field :label="$t('home.search.purchase')" position="is-centered" expanded>
                                    <b-datepicker class="is-dark" ref="dateRef" expanded v-model="searchDates" :focused-date="new Date()"
                                                  icon="calendar-alt" :placeholder="$t('home.search.purchase-info')" :years-range="[-10, 10]" range/>
                                </b-field>

                                <b-field :label="$t('home.search.classification')" position="is-centered" expanded>
                                    <b-autocomplete ref="cityRef" v-model="searchClassificationText" :placeholder="$t('home.search.classification-info')" 
                                        :open-on-focus="openOnFocusClassification" :keep-first="true" :custom-formatter="dataFormatter()"
                                        :data="filteredClassifications" @select="option => searchClassification = option" :clearable="true"
                                        dropdown-position="bottom" autoComplete="new-password">

                                        <template slot-scope="props">
                                            <p>
                                                <span class="has-text-semibold">{{ props.option.id }}</span>
                                                <span> - </span>
                                                <span>{{ props.option.name }}</span>
                                            </p>
                                        </template>

                                        <template slot="empty">{{ $t('home.search.no-result') }} {{ searchClassificationText }}</template>
                                    </b-autocomplete>
                                </b-field>

                                <b-field :label="$t('home.search.seller')" position="is-centered" expanded v-if="isAdmin">
                                    <b-autocomplete ref="cityRef" v-model="searchSellerText" :placeholder="$t('home.search.seller-info')" 
                                        :open-on-focus="openOnFocusSeller" :keep-first="true" :custom-formatter="sellerFormatter()"
                                        :data="filteredSellers" @select="option => searchSeller = option" :clearable="true"
                                        dropdown-position="bottom" autoComplete="new-password">

                                        <template slot-scope="props">
                                            <p>
                                                <span class="has-text-semibold">{{ props.option.id }}</span>
                                                <span> - </span>
                                                <span>{{ props.option.email }}</span>
                                            </p>
                                        </template>

                                        <template slot="empty">{{ $t('home.search.no-result') }} {{ searchSellerText }}</template>
                                    </b-autocomplete>
                                </b-field>
                            </b-field>

                            <div class="level is-mobile has-top-margin">
                                <div class="level-left">
                                    <!-- Leave in blank to make the right side stick to the side -->
                                </div>

                                <div class="level level-right">
                                    <div class="level-item">
                                        <b-button type="is-dark" icon-left="search" @click="search()">
                                            {{ $t('home.search.search') }}
                                        </b-button>
                                    </div>

                                    <div class="level-item">
                                        <b-button type="is-dark" icon-left="times" @click="clear()">
                                            {{ $t('home.search.clear') }}
                                        </b-button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <hr/>

                        <div>
                            <b-table ref="tableCustomers" class="has-margin-table" :data="customers" hoverable
                                :default-sort-direction="defaultSortOrder" :default-sort="[sortField, sortOrder]">
                                <template slot-scope="props">
                                    <b-table-column class="is-unselectable" cell-class="is-vertical" field="classification.name" :label="$t('home.list.classification')">
                                        {{props.row.classification.name}}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" cell-class="is-vertical" field="name" :label="$t('home.list.name')">
                                        {{props.row.name}}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" cell-class="is-vertical" field="phone" :label="$t('home.list.phone')">
                                        {{props.row.phone}}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" width="90" cell-class="is-vertical" field="gender.name" :label="$t('home.list.gender')">
                                        {{props.row.gender.name }}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" cell-class="is-vertical" field="city.region.name" :label="$t('home.list.region')">
                                        {{props.row.city.region.name }}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" cell-class="is-vertical" field="city.name" :label="$t('home.list.city')">
                                        {{props.row.city.name }}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" width="150" cell-class="is-vertical" field="lastPurchase" :label="$t('home.list.purchase')">
                                        {{props.row.lastPurchase | shortDate() }}
                                    </b-table-column>

                                    <b-table-column class="is-unselectable" cell-class="is-vertical" field="user.email" :label="$t('home.list.seller')" :visible="isAdmin">
                                        {{props.row.user.email }}
                                    </b-table-column>
                                </template>

                                <template slot="empty">
                                    <div class="content has-text-grey has-text-centered has-bottom-margin">
                                        <p>{{ $t('home.list.none') }}</p>
                                    </div>
                                </template>
                            </b-table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'Home',

        data() {
            return {
                isLoading: true,
                isAdmin: false,

                searchName: '',
                searchGenderText: '',
                searchGender: {},
                searchRegionText: '',
                searchRegion: {},
                searchCityText: '',
                searchCity: {},
                searchClassificationText: '',
                searchClassification: {},
                searchSellerText: '',
                searchSeller: {},
                searchDates: [],

                users: [],
                genders: [],
                regions: [],
                cities: [],
                classifications: [],
                customers: [],

                sortField: "properties.customer.name",
                sortOrder: "asc",
                defaultSortOrder: "asc",
            }
        },

        async mounted() {
            try {
                console.log('State', this.$store.state.auth);
                this.isAdmin = this.$store.state.auth.user.isAdmin;
                
                var response = await this.$store.dispatch('data/getAll', { path: 'user' });
                this.users = response.content;

                response = await this.$store.dispatch('data/getAll', { path: 'gender' });
                this.genders = response.content;
                response = await this.$store.dispatch('data/getAll', { path: 'region' });
                this.regions = response.content;
                response = await this.$store.dispatch('data/getAll', { path: 'city' });
                this.cities = response.content;
                response = await this.$store.dispatch('data/getAll', { path: 'classification' });
                this.classifications = response.content;
                
                await this.search();
            } catch (e) {
                console.log('Error in getting events', e);
                this.$router.push({ path: '/signin', query: { next: '/' } })
            }
            finally{
                this.isLoading = false;
            }
        },

        filters: {
            shortDate(dateString) {
                var date = new Date(dateString);

                return (date.getDate() < 10 ? "0" : '') + date.getDate() + '/' + (date.getMonth() + 1 < 10 ? "0" : '') + (date.getMonth() + 1) + '/' + date.getFullYear();
            },
        },

        methods: {
            isEmpty(obj) {
                for(var i in obj) 
                    return false; 
    
                return true;
            },
            dataFormatter() {
                return (e) => { return e.id + " - " + e.name; };
            },
            sellerFormatter() {
                return (e) => { return e.id + " - " + e.email; };
            },
            regionSelected(option) {
                console.log('Region selected');
                this.searchRegion = option;

                if (this.searchRegion && this.searchCity && this.searchRegion.id !== this.searchCity.regionId) {
                    this.searchCity = {};
                    this.searchCityText = '';
                }
            },
            async search() {
                var params = {
                    name: this.searchName,
                    gender: this.searchGender ? this.searchGender.id : null,
                    region: this.searchRegion ? this.searchRegion.id : null,
                    city: this.searchCity ? this.searchCity.id : null,
                    classification: this.searchClassification ? this.searchClassification.id : null,
                    seller: this.searchSeller ? this.searchSeller.id : null,
                    purchaseStart: this.searchDates.length > 0 ? this.searchDates[0] : null, 
                    purchaseEnd: this.searchDates.length > 1 ? this.searchDates[1] : null
                };

                var response = await this.$store.dispatch('customer/getCustomers', { params });
                this.customers = response;
            },
            clear(){
                this.searchName = '';
                this.searchGenderText = '';
                this.searchGender = {};
                this.searchRegionText = '';
                this.searchRegion = {};
                this.searchCityText = '';
                this.searchCity = {};
                this.searchClassificationText = '';
                this.searchClassification = {};
                this.searchSellerText = '';
                this.searchSeller = {};
                this.searchDates = [];
            }
        },

        computed: {
            openOnFocusGender() {
                //If no gender was selected yet or if the text is not equal to the selection, show the result.
                return this.isEmpty(this.searchGender) || this.searchGender == null || this.searchGenderText !== (this.searchGender.id + " - " + this.searchGender.name);
            },
            filteredGenders() {
                let text = this.searchGenderText.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

                return this.genders ? this.genders.filter(gender => {
                    return (gender.id + " - " + gender.name).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').indexOf(text) >= 0;
                }).slice(0, 20) : [];
            },
            openOnFocusRegion() {
                //If no region was selected yet or if the text is not equal to the selection, show the result.
                return this.isEmpty(this.searchRegion) || this.searchRegion == null || this.searchRegionText !== (this.searchRegion.id + " - " + this.searchRegion.name);
            },
            filteredRegions() {
                let text = this.searchRegionText.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

                return this.regions ? this.regions.filter(region => {
                    return (region.id + " - " + region.name).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').indexOf(text) >= 0;
                }).slice(0, 20) : [];
            },
            openOnFocusCity() {
                //If no region was selected yet or if the text is not equal to the selection, show the result.
                return this.isEmpty(this.searchCity) || this.searchCity == null || this.searchCityText !== (this.searchCity.id + " - " + this.searchCity.name);
            },
            filteredCities() {
                let text = this.searchCityText.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

                return this.cities ? this.cities.filter(city => {
                    return (city.id + " - " + city.name).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').indexOf(text) >= 0 
                    && (this.isEmpty(this.searchRegion) || city.regionId === this.searchRegion.id);
                }).slice(0, 20) : [];
            },
            openOnFocusClassification() {
                //If no classification was selected yet or if the text is not equal to the selection, show the result.
                return this.isEmpty(this.searchClassification) || this.searchClassification == null || this.searchClassificationText !== (this.searchClassification.id + " - " + this.searchClassification.name);
            },
            filteredClassifications() {
                let text = this.searchClassificationText.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

                return this.classifications ? this.classifications.filter(classification => {
                    return (classification.id + " - " + classification.name).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').indexOf(text) >= 0;
                }).slice(0, 20) : [];
            },
            openOnFocusSeller() {
                //If no seller was selected yet or if the text is not equal to the selection, show the result.
                return this.isEmpty(this.searchSeller) || this.searchSeller == null || this.searchSellerText !== (this.searchSeller.id + " - " + this.searchSeller.email);
            },
            filteredSellers() {
                let text = this.searchSellerText.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '');

                return this.users ? this.users.filter(seller => {
                    return (seller.id + " - " + seller.email).toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').indexOf(text) >= 0;
                }).slice(0, 20) : [];
            },
        },
    }
</script>

<style lang="scss" scoped>
    .has-side-margin {
        margin: 10px 0;
    }

    .columns {
        margin-top: -60px;
    }

    .has-shadow {
        border-radius: 2px;
        box-shadow: 0px 0px 16px rgba(0, 0, 0, 0.06);
    }

    .column .is-padded {
        padding: 0.75rem;
    }

    .column .button {
        white-space: normal;
        border-radius: 2px;
        width: 100%;
        height: 100%;
    }

    .limits-with-ellipses {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
</style>

<style lang="scss">
    .button > span {
        width: 100%;
        padding: 0px 5px;
    }
</style>