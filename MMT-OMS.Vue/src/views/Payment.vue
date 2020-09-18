<template>
    <v-container>
        <v-row justify="center">
            <v-col cols="12" md="8">
                <h3 class="text-h3">Upload your payment</h3>
                <v-divider class="my-5"></v-divider>
                <v-form ref="form">
                    <v-row>
                        <v-col cols="12" md="6">
                            <v-text-field label="Order Code" v-bind:rules="requiredRules" v-model="uniqueCode"></v-text-field>
                        </v-col>
                        <v-col cols="12" md="6">
                            <v-file-input v-model="files" label="Payment Images" multiple prepend-icon="mdi-paperclip" v-bind:rules="requiredRules">
                                <template v-slot:selection="{ text }">
                                    <v-chip small label color="primary">
                                        {{ text }}
                                    </v-chip>
                                </template>
                            </v-file-input>
                        </v-col>
                    </v-row>
                    <v-btn class="purple--text" outlined v-on:click="upload" v-bind:loading="uploadIsLoading">Upload</v-btn>
                </v-form>
            </v-col>
        </v-row>

        <!-- dialogs -->
        <v-dialog v-model="hasError" v-on:click:outside="hasError = false" min-width="290" max-width="400">
            <v-card>
                <v-card-title class="headline">Error</v-card-title>
                <v-card-text class="body-1">{{ errorMessage }}</v-card-text>
                <v-card-actions>
                    <v-btn color="green darken-1" text v-on:click="hasError = false">
                        Ok
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <v-dialog v-model="isSuccess" min-width="290" max-width="400" persistent>
            <v-card>
                <v-card-title class="headline">Success</v-card-title>
                <v-card-text class="body-1">Your payment info has been submitted.</v-card-text>
                <v-card-actions>
                    <v-btn color="green darken-1" text v-on:click="router.go(0)">
                        Ok
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-container>
</template>
<script>
export default {
    data(){
        return{
            files: null,
            uniqueCode: '',
            requiredRules : [
                v => !!v || `This can't be empty.`
            ],
            uploadIsLoading: false,
            errorMessage: '',
            hasError: false,
            isSuccess: false,
        }
    }, 
    methods: {
        upload(){
            const isValid = this.$refs.form.validate();
            if(!isValid){
                return;
            }

            const formData = new FormData();
            formData.append('uniqueCode', this.uniqueCode);
            this.files.forEach(file => {
                formData.append('file', file);
            });

            this.uploadIsLoading = true;
            const that = this;
            this.$axios({
                crossdomain: false,
                method: 'post',
                url: 'api/customer-order/submit-payment',
                data: formData,
                headers: { 'content-type': 'multipart/form-data' }
            })
            .then(() => that.isSuccess = true)
            .catch(error => {
                that.hasError = true;
                if (error.response) {
                    that.errorMessage = error.response.data.Message;
                } else if (error.request) {
                    // The request was made but no response was received
                    console.log(error.request);
                } else {
                    that.errorMessage = error.message;
                }
            }).finally(() => {
                that.uploadIsLoading = false;
            });
        }
    }
}
</script>