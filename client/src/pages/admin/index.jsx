import React, { useEffect, useState } from 'react'
import MainAdLayout from 'src/pages/admin/mainAdLayout'
import Console from './Console'
import Page from 'src/components/Page';
import { useRouter } from 'next/router';
import { useRecoilValue } from 'recoil';
import { dataUser } from 'src/recoils/dataUser';
import Loading from 'src/sections/auth/Loading';


User.getLayout = (page) => <MainAdLayout>{page}</MainAdLayout>






// nếu ko valid => Chuyển nó về trang login

// nhận kết quả trả về từ server => Lưu vào localstorage



export default function User() {

    const router = useRouter();
    const user = useRecoilValue(dataUser)
    const [loading, setLoading] = useState(false)


    if (loading) {
        return <Loading />
    } else {


        return (

            (<div className='bg-[#f8f9fd]'>
                <Page
                    data={{
                        title: 'Miki Shop Admin',
                        description: '',
                        url: '',
                        thumbnailUrl: '',
                    }}
                />
                <Console />
            </div>)
        )
    }

}
